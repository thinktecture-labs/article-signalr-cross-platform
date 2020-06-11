import { animate, state, style, transition, trigger } from '@angular/animations';
import { Component, HostBinding, OnDestroy, OnInit } from '@angular/core';
import { NotificationService } from '../../services/notification.service';
import { triggerChildAnimation } from '../animations/triggerChildAnimation';
import { verticalCollapseAnimation } from '../animations/verticalCollapseAnimation';

@Component({
  selector: 'sr-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss'],
  // REVIEW: Warum sind die anderen zwei Animationen ausgelagert, aber die dritte nicht?
  animations: [
    trigger('slideIn', [
      state('*', style({
        transform: 'translateY(0) scale(1) rotateY(0)',
        opacity: 1,
        filter: 'blur(0) saturate(100%)'
      })),
      state('void', style({
        transform: 'translateY(20px) scale(1.1) rotateY(5deg)',
        opacity: 0,
        filter: 'blur(2px) saturate(50%)'
      })),
      transition('void => *',  animate('.3s ease-in-out')),
    ]),
    trigger('slideOut', [
      state('*', style({
        transform: 'translateX(0)  scale(1)',
        opacity: 1,
      })),
      state('void', style({
        transform: 'translateX(100%) scale(.7)',
        opacity: 0,
      })),
      transition('* => void', animate('.2s ease')),
    ]),
    verticalCollapseAnimation,
    triggerChildAnimation,
  ],
})
export class NotificationsComponent implements OnInit, OnDestroy {
  private subscription: any;

  // REVIEW: Hier hat der Decorator einen anderen Codestyle als in den anderen Klassen, warum?
  @HostBinding('class.open') hasItems: boolean;

  constructor(public toastService: NotificationService) {
  }

  ngOnInit(): void {
    this.subscription = this.toastService.hasItems$.subscribe(hasItems => this.hasItems = hasItems);
  }

  ngOnDestroy(): void {
    this.subscription?.unsubscribe();
  }
}
