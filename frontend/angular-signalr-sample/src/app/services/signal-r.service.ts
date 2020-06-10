import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { OAuthService } from 'angular-oauth2-oidc';
import { Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { Toast } from '../models/toast';
import { User } from '../models/user';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;
  public userPlayed$ = new Subject<number>();
  public resetGame$ = new Subject<void>();
  public userOnline$ = new Subject<User>();
  public userOffline$ = new Subject<User>();
  public ownUser$ = new Subject<User>();

  constructor(
    private readonly oAuthService: OAuthService,
    private readonly notificationService: NotificationService,
  ) {
  }

  public get state() {
    return this.hubConnection?.state;
  }

  public startConnection = async () => {
    const token = this.oAuthService.getAccessToken();
    if (!token) {
      await this.notify('Es konnte keine Verbindung aufgebaut werden, da Sie noch nicht angemeldet sind.');
      return;
    }
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiBaseUrl}tictactoe`, {
        accessTokenFactory: () => token
      })
      .withAutomaticReconnect([0, 5000, 10000])
      .build();
    await this.hubConnection.start();
    await this.notify('Erfolgreich am Hub angemeldet!');
    const user = await this.hubConnection.invoke('OwnConnectionId');
    this.ownUser$.next(user);

    this.addTransferUserConnectedListener();
    this.addTransferUserDisconnectedListener();
    this.addTransferPlayRoundListener();
    this.addTransferResetListener();
    this.addReconnectListener();
  };

  public async sendPlayRound(data: number) {
    console.log('Send data', data);
    await this.hubConnection.invoke('PlayRound', `${data}`);
  }

  public async resetRound() {
    await this.hubConnection.invoke('ResetGame');
  }

  private addReconnectListener = () => {
    this.hubConnection.onreconnected(_ => {
      this.notify('Die Verbindung wurde wieder hergestellt').catch(err => console.log(err));
    })
  }

  private addTransferUserConnectedListener = () => {
    this.hubConnection.on('UserConnected', (data) => {
      this.userOnline$.next(data);
    });
  };

  private addTransferUserDisconnectedListener = () => {
    this.hubConnection.on('UserDisconnected', (data) => {
      this.userOffline$.next(data);
    });
  };

  private addTransferPlayRoundListener = () => {
    this.hubConnection.on('Play', (data) => {
      console.log('User played its your turn');
      this.userPlayed$.next(data);
    });
  };

  private addTransferResetListener = () => {
    this.hubConnection.on('Reset', () => {
      console.log('User reset the game');
      this.resetGame$.next(void 0);
    });
  };

  private async notify(payload) {
    const toast = {
      title: payload,
    } as Toast;
    this.notificationService.addNotification(toast);
    setTimeout(() => {
      this.notificationService.removeNotification(toast);
    }, 1000);
  }
}
