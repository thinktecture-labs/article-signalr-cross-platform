import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { OAuthService } from 'angular-oauth2-oidc';
import { Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { Toast } from '../models/toast';
import { User } from '../models/user';
import { NotificationService } from './notification.service';

// REVIEW: Macht es Sinn, dem SignalR Service die Spielevents bekannt zu geben?
// So, wie er benannt ist, ist es eigentlich generisch zum Aufbauen der Verbindung da.
// Entweder würde ich ihn dann umbenennen oder einen weiteren Service implementieren,
// der die Spielevents kennt und diese mit dem SignalRService dann verarbeiten kann.

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

  // REVIEW: Gibt's einen Grund, warum die Funktion hier als Feld definiert ist und nicht als echte Funktion?
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

  // REVIEW: Gibt's einen Grund, warum alle die Funktionen hier nicht als echte Funktion sondern als Felder deklariert sind?
  private addReconnectListener(): void {
    this.hubConnection.onreconnected(_ => {
      this.notify('Die Verbindung wurde wieder hergestellt').catch(err => console.log(err));
    });
  }

  private addTransferUserConnectedListener(): void {
    this.hubConnection.on('UserConnected', (data) => {
      this.userOnline$.next(data);
    });
  };

  private addTransferUserDisconnectedListener(): void {
    this.hubConnection.on('UserDisconnected', (data) => {
      this.userOffline$.next(data);
    });
  };

  private addTransferPlayRoundListener(): void {
    this.hubConnection.on('Play', (data) => {
      console.log('User played its your turn');
      this.userPlayed$.next(data);
    });
  };

  private addTransferResetListener(): void {
    this.hubConnection.on('Reset', () => {
      console.log('User reset the game');
      this.resetGame$.next(void 0);
    });
  };

  // REVIEW: Warum muss der SignalR-Service die Toast Notification wieder löschen?
  // Das ist definitiv die Zuständigkeit vom NotificationService. CHECK
  // Zudem fehlt das Typing für payload. CHECK
  // Und Payload sollte eher title heißen? CHECK
  private async notify(title: string): Promise<void> {
    const toast = {
      title,
    } as Toast;
    this.notificationService.showNotification(toast);
  }
}
