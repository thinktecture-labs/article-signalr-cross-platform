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
  // REVIEW: Warum sind die anderen zwei Animationen ausgelagert, aber die dritte nicht? CHECK
  animations: [
    slideInAnimation,
    slideOutAnimation,
    verticalCollapseAnimation,
    triggerChildAnimation,
  ],
})
export class NotificationsComponent implements OnInit, OnDestroy {
  private subscription: Subscription;

  // REVIEW: Hier hat der Decorator einen anderen Codestyle als in den anderen Klassen, warum? CHECK
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
