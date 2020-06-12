import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { User } from '../../models/user';
import { SignalRService } from '../../services/signal-r.service';
import { UsersService } from '../../services/users.service';

@Component({
  selector: 'sr-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  public users$: BehaviorSubject<User[]>;

  constructor(private readonly signalRService: SignalRService, public readonly usersService: UsersService) { }

  public async ngOnInit(): Promise<void> {
    await this.signalRService.startConnection();
    this.users$ = this.usersService.users$;
  }
}
