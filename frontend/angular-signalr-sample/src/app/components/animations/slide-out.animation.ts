import { animate, state, style, transition, trigger } from '@angular/animations';

export const slideOutAnimation = trigger('slideOut', [
  state('*', style({
    transform: 'translateX(0)  scale(1)',
    opacity: 1,
  })),
  state('void', style({
    transform: 'translateX(100%) scale(.7)',
    opacity: 0,
  })),
  transition('* => void', animate('.2s ease')),
]);
