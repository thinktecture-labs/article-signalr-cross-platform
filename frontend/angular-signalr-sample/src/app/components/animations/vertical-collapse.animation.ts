import { animate, state, style, transition, trigger } from '@angular/animations';

export const verticalCollapseAnimation = trigger('verticalCollapse', [
  state('*', style({
    height: '*',
  })),
  state('void', style({
    height: '0',
  })),
  transition('* => void', animate('.3s .3s ease')),
]);
