import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { Toast } from '../models/toast';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notifications: Toast[] = [];

  public items$: Observable<Toast[]>;
  public hasItems$ = new BehaviorSubject<boolean>(false);

  constructor() {
    this.items$ = of(this.notifications);
  }

  public showNotification(title: string, duration: number = 2000): void {
    const toast = {
      title,
    } as Toast;
    this.notifications.push(toast);
    this.hasItems$.next(this.notifications.length > 0);

    setTimeout(() => {
      this.removeNotification(toast);
    }, duration);
  }

  private removeNotification(toast: Toast): void {
    const index = this.notifications.indexOf(toast);
    if (index > -1) {
      this.notifications.splice(index, 1);
    }
    this.hasItems$.next(this.notifications.length > 0);
  }
}
