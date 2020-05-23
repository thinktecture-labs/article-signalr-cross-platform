import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SignalRService } from '../../services/signal-r.service';
import { UsersService } from '../../services/users.service';

@Component({
  selector: 'sr-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public users$: BehaviorSubject<any[]>;


  constructor(private readonly signalRService: SignalRService, public readonly usersService: UsersService) { }

  async ngOnInit(): Promise<void> {
    this.signalRService.startConnection();
    this.users$ = this.usersService.users$;
    await this.usersService.start();
  }

}
