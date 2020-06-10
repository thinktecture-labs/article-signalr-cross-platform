import { Component, EventEmitter, HostListener, Input, Output } from '@angular/core';

@Component({
  selector: 'sr-cell',
  templateUrl: './cell.component.html',
  styleUrls: ['./cell.component.scss'],
})
export class CellComponent {
  @Input()
  public value: string;

  @Output('userClick')
  public click = new EventEmitter<void>();

  @HostListener('click')
  clickHandler() {
    this.click.emit(void 0);
  }
}
