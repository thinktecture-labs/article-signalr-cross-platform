import { animate, state, style, transition, trigger } from '@angular/animations';

export const slideInAnimation = trigger('slideIn', [
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
]);
