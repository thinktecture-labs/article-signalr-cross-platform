import { Component, HostBinding, Input } from '@angular/core';

@Component({
  selector: 'sr-icon',
  templateUrl: './icon.component.html',
  styleUrls: ['./icon.component.scss'],
})
export class IconComponent {

  @Input()
  public name: string;

  @HostBinding('class.disabled')
  @Input()
  public disabled: boolean;

  constructor() {
  }
}
