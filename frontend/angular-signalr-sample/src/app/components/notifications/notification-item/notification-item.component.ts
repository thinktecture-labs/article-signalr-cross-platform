import { Component, Input, OnInit } from '@angular/core';
import { Toast } from '../../../models/toast';

@Component({
  selector: 'sr-notification-item',
  templateUrl: './notification-item.component.html',
  styleUrls: ['./notification-item.component.scss'],
})
export class NotificationItemComponent implements OnInit {
  // REVIEW: Hier würde ein public fehlen oder bei allen anderen publics eben entfernen.
  @Input()
  toast: Toast;

  // REVIEW: Ist leer, kann weg
  constructor() {
  }

  // REVIEW: Ist leer, kann weg
  ngOnInit(): void {
  }

}

