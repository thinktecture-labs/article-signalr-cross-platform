import { Component, Input, OnInit } from '@angular/core';
import { Toast } from '../../../models/toast';

@Component({
  selector: 'sr-notification-item',
  templateUrl: './notification-item.component.html',
  styleUrls: ['./notification-item.component.scss'],
})
export class NotificationItemComponent implements OnInit {
  @Input()
  toast: Toast;

  constructor() {
  }

  ngOnInit(): void {
  }

}

