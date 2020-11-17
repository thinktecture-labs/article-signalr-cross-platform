import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { BehaviorSubject, Subject } from 'rxjs';
import { GameSession } from '../models/game-session';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  public userPlayed$ = new Subject<number>();
  public gameRunning$ = new BehaviorSubject<boolean>(false);
  public gameOver$ = new BehaviorSubject<string>('');
  public activeSession$ = new BehaviorSubject<GameSession>(null);

  constructor(
    private readonly oAuthService: OAuthService
  ) {
  }

  public get state() {
    return null;
  }

  public get connectionId() {
    return '';
  }

  public async startConnection(): Promise<void> {
  };
}
