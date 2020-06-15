import { HttpClient } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { BehaviorSubject, combineLatest, Subscription } from 'rxjs';
import { map, tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';
import { User } from '../models/user';
import { SignalRService } from './signal-r.service';

@Injectable({
  providedIn: 'root',
})
export class UsersService implements OnDestroy{
  private subscription: Subscription = new Subscription();
  private users: User[] = [];
  public users$ = new BehaviorSubject<User[]>([]);

  constructor(
    private readonly signalRService: SignalRService,
    private readonly oAuthService: OAuthService,
    private readonly httpClient: HttpClient,
  ) {
    this.init();
  }

  public ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  private init() {
    combineLatest([this.httpClient.get<User[]>(`${environment.apiBaseUrl}users`), this.signalRService.ownUser$])
      .pipe(
        map(([users, ownUser]) => users.filter(u => u.connectionId !== ownUser.connectionId)),
        tap(users => console.log(users)),
      ).subscribe(users => {
      this.users = users ?? [];
      this.users$.next(this.users);
    });
  }
}
