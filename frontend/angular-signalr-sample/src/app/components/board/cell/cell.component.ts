import { Component, EventEmitter, HostListener, Input, Output } from '@angular/core';

@Component({
  selector: 'sr-cell',
  templateUrl: './cell.component.html',
  styleUrls: ['./cell.component.scss'],
})
export class CellComponent {
  @Input()
  public value: string;

  @Output()
  public userClick = new EventEmitter<void>();

  @HostListener('click')
  public clickHandler() {
    this.userClick.emit(void 0);
  }
}
