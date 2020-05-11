import { Injectable } from '@angular/core';
import * as signalR from '@aspnet/signalr';
import { OAuthService } from 'angular-oauth2-oidc';
import { Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { Toast } from '../models/toast';
import { NotificationService } from './notification.service';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;
  public userPlayed$ = new Subject<number>();
  public resetGame$ = new Subject<void>();
  public userOnline$ = new Subject<void>();

  constructor(
    private readonly oAuthService: OAuthService,
    private readonly notificationService: NotificationService,
  ) {
  }

  public get state() {
    return this.hubConnection?.state;
  }

  public startConnection = () => {
    const token = this.oAuthService.getAccessToken();
    if (!token) {
      return;
    }
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(`${environment.apiBaseUrl}notifications`, {
        accessTokenFactory: () => token
      })
      .build();

    this.hubConnection
      .start()
      .then(() => {
        this.notify('Erfolgreich am Hub angemeldet!').catch(err => console.log(err));
      })
      .catch(err => console.log('Error while starting connection: ' + err));

    this.addTransferNotificationsListener();
    this.addTransferPlayroundListener();
    this.addTransferUsersListener();
    this.addTransferResetListener();
  };

  public addTransferNotificationsListener = () => {
    this.hubConnection.on('Notifications', (data) => {
      this.notify(data);
    });
  };

  public addTransferUsersListener = () => {
    this.hubConnection.on('UserConnected', (data) => {
      this.userOnline$.next(void 0);
    });
  };

  public addTransferPlayroundListener = () => {
    this.hubConnection.on('Play', (data) => {
      console.log('User played its your turn');
      this.userPlayed$.next(data);
    });
  };

  public addTransferResetListener = () => {
    this.hubConnection.on('Reset', () => {
      console.log('User reset the game');
      this.resetGame$.next(void 0);
    });
  };

  public async sendPlayRound(data: number) {
    console.log('Send data', data);
    await this.hubConnection.invoke('PlayRound', `${data}`);
  }

  public async resetRound() {
    await this.hubConnection.invoke('ResetGame');
  }

  private async notify(payload) {
    const toast = {
      title: payload,
    } as Toast;
    this.notificationService.addNotification(toast);
    setTimeout(() => {
      this.notificationService.removeNotification(toast);
    }, 2500);
  }
}
