import { animate, animateChild, query, transition, trigger } from '@angular/animations';

export const triggerChildAnimation = trigger('triggerChildAnimation', [
  transition(':enter, :leave', [animate('0s'), query('*', [animateChild()])]),
]);

