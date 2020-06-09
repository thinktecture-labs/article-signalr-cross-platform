import { Component, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { SignalrService } from '../../services/signalr.service';
import { UsersService } from '../../services/users.service';

@Component({
  selector: 'sr-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  public users$: BehaviorSubject<any[]>;


  constructor(private readonly signalRService: SignalrService, public readonly usersService: UsersService) { }

  async ngOnInit(): Promise<void> {
    await this.signalRService.startConnection();
    this.users$ = this.usersService.users$;
  }
}
