import { Component, OnInit } from '@angular/core';
import { HubConnectionState } from '@microsoft/signalr';
import { OAuthService } from 'angular-oauth2-oidc';
import { DeviceDetectorService } from 'ngx-device-detector';
import { authConfig } from './auth.config';
import { SignalRService } from './services/signal-r.service';

@Component({
  selector: 'sr-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  public states = HubConnectionState;
  public isMobileDevice: boolean;
  public isTabletDevice: boolean;

  public get connectionState(): HubConnectionState {
    return this.signalRService.state;
  }

  constructor(
    private readonly oauthService: OAuthService,
    private readonly signalRService: SignalRService,
    private readonly deviceService: DeviceDetectorService
  ) {
    this.configureCodeFlow();
  }

  public ngOnInit(): void {
    this.isMobileDevice = this.deviceService.isMobile();
    this.isTabletDevice = this.deviceService.isTablet();
  }

  private configureCodeFlow(): void {
    this.oauthService.configure(authConfig);
  }
}
