import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { NotificationService } from '../../services/notification.service';
import { slideInAnimation } from '../animations/slide-in.animation';
import { slideOutAnimation } from '../animations/slide-out.animation';
import { triggerChildAnimation } from '../animations/trigger-child.animation';
import { verticalCollapseAnimation } from '../animations/vertical-collapse.animation';

@Component({
  selector: 'sr-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss'],
  animations: [
    slideInAnimation,
    slideOutAnimation,
    verticalCollapseAnimation,
    triggerChildAnimation,
  ],
})
export class NotificationsComponent implements OnInit, OnDestroy {
  private subscription: Subscription;

  @HostBinding('class.open')
  public hasItems: boolean;

  constructor(public toastService: NotificationService) {
  }

  public ngOnInit(): void {
    this.subscription = this.toastService.hasItems$.subscribe(hasItems => this.hasItems = hasItems);
  }

  public ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
