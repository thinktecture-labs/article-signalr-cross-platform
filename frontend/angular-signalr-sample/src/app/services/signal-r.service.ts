import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { OAuthService } from 'angular-oauth2-oidc';
import { BehaviorSubject, Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { GameSession } from '../models/game-session';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;
  public userPlayed$ = new Subject<number>();
  public gameRunning$ = new BehaviorSubject<boolean>(false);
  public gameOver$ = new BehaviorSubject<string>('');
  public activeSession$ = new BehaviorSubject<GameSession>(null);

  constructor(
    private readonly oAuthService: OAuthService,
    private readonly notificationService: NotificationService,
  ) {
  }

  public get state() {
    return this.hubConnection?.state;
  }

  public get connectionId() {
    return this.hubConnection?.connectionId;
  }

  public async startConnection(): Promise<void> {
    const token = this.oAuthService.getAccessToken();
    if (!token) {
      this.notificationService.showNotification('Es konnte keine Verbindung aufgebaut werden, da Sie noch nicht angemeldet sind.');
      return;
    }
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiBaseUrl}tictactoe`, {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect([0, 5000, 10000])
      .build();
    await this.hubConnection.start();
    this.notificationService.showNotification('Erfolgreich am Hub angemeldet!');

    this.addStartGameListener();
    this.addGameOverListener();
    this.addTransferPlayRoundListener();
    this.addReconnectListener();
    await this.joinNewSession();
  };

  public async sendPlayRound(data: number) {
    await this.hubConnection.invoke('PlayRound', data);
  }

  public async joinNewSession() {
    await this.hubConnection.invoke('JoinSession');
    this.gameOver$.next('');
  }

  private addReconnectListener(): void {
    this.hubConnection.onreconnected(_ => {
      this.notificationService.showNotification('Die Verbindung wurde wieder hergestellt');
    });
  }

  private addStartGameListener(): void {
    this.hubConnection.on('StartGame', (session: GameSession) => {
      this.gameRunning$.next(true);
      this.activeSession$.next(session);
      this.notificationService.showNotification('Das Spiel beginnt :-)');
    });
  }

  private addGameOverListener(): void {
    this.hubConnection.on('GameOver', result => {
      this.gameRunning$.next(false);
      this.gameOver$.next(result);
      this.activeSession$.next(null);
      this.notificationService.showNotification(`Das Spiel ist vorbei. ${result}`);
    });
  }

  private addTransferPlayRoundListener(): void {
    this.hubConnection.on('Play', (data) => {
      this.userPlayed$.next(data);
    });
  };
}
