import { Component, OnInit } from '@angular/core';
import { HubConnectionState } from '@microsoft/signalr';
import { OAuthService } from 'angular-oauth2-oidc';
import { DeviceDetectorService } from 'ngx-device-detector';
import { filter } from 'rxjs/operators';
import { authConfig } from './auth.config';
import { SignalrService } from './services/signalr.service';

@Component({
  selector: 'sr-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  public connectionState: HubConnectionState = this.signalRService.state;
  public isMobileDevice: boolean;
  public isTabletDevice: boolean;

  constructor(
    private readonly oauthService: OAuthService,
    private readonly signalRService: SignalrService,
    private readonly deviceService: DeviceDetectorService
  ) {
    this.configureCodeFlow();
  }

  ngOnInit(): void {
    this.isMobileDevice = this.deviceService.isMobile();
    this.isTabletDevice = this.deviceService.isTablet();
    this.oauthService.events
      .pipe(filter(e => e.type === 'token_received'))
      .subscribe(_ => {
        this.oauthService.loadUserProfile().catch(err => console.log(err));
      });
  }

  private configureCodeFlow() {
    if (authConfig.redirectUri === 'tictactoe:///callback') {
      authConfig.redirectUri = 'tictactoe://localhost/callback';
    }
    this.oauthService.configure(authConfig);
  }
}
