import { Component, OnInit } from '@angular/core';
import { HubConnectionState } from '@aspnet/signalr';
import { Subject } from 'rxjs';
import { SignalRService } from '../../services/signal-r.service';
import { UsersService } from '../../services/users.service';

@Component({
  selector: 'sr-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public userOnline$: Subject<boolean>;


  constructor(private readonly signalRService: SignalRService, public readonly usersService: UsersService) { }

  ngOnInit(): void {
    this.signalRService.startConnection();
    this.userOnline$ = this.usersService.userOnline$;
    this.usersService.start();
    /*setTimeout(() => {
      if (this.signalRService.state === HubConnectionState.Connected) {
        this.usersService.start();
      }
    }, 5000);*/
  }

}
