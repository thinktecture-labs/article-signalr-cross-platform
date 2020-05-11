import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { environment } from '../../environments/environment';
import { SignalRService } from './signal-r.service';

@Injectable({
  providedIn: 'root'
})
export class UsersService {
  public userOnline$ = new Subject<boolean>();

  constructor(private readonly signalrRService: SignalRService, private readonly httpClient: HttpClient) {
  }

  public async start() {
    const users = await this.httpClient.get<any[]>(`${environment.apiBaseUrl}users`).toPromise();
    if (users.length > 0) {
      this.userOnline$.next(true);
    }
    this.signalrRService.userOnline$.subscribe(() => this.userOnline$.next(true));
  }
}
