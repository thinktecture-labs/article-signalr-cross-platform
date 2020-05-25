import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { BehaviorSubject, combineLatest } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { User } from '../models/user';
import { SignalrService } from './signalr.service';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  private users: User[];
  public users$ = new BehaviorSubject<User[]>([]);

  constructor(
    private readonly signalRService: SignalrService,
    private readonly oAuthService: OAuthService,
    private readonly httpClient: HttpClient,
  ) {
  }

  public async start() {
    combineLatest([this.httpClient.get<User[]>(`${environment.apiBaseUrl}users`), this.signalRService.ownUser$])
      .pipe(map(([users, ownUser]) => {
        return users.filter(u => u.connectionId !== ownUser.connectionId);
      })).subscribe(users => {
        this.users = users;
        this.users$.next(this.users);
    });
    this.signalRService.userOnline$.subscribe((user: User) => {
      const currentUserIndex = this.users.findIndex(u => u.name === user.name);
      if (currentUserIndex < 0) {
        this.users.push(user);
        this.users$.next(this.users);
      }
    });
    this.signalRService.userOffline$.subscribe((user) => {
      const currentUserIndex = this.users.findIndex(u => u.name === user.name);
      if (currentUserIndex > -1) {
        this.users.splice(currentUserIndex, 1);
        this.users$.next(this.users);
      }
    });
  }
}
