import { Component, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { SignalRService } from '../../services/signal-r.service';

@Component({
  selector: 'sr-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  constructor(private readonly signalRService: SignalRService) { }

  ngOnInit(): void {
    this.signalRService.startConnection();
  }

}
