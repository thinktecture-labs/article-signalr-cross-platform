import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { filter } from 'rxjs/operators';
import { authConfig } from './auth.config';
import { SignalRService } from './services/signal-r.service';

@Component({
  selector: 'sr-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {

  constructor(private router: Router,
              private oauthService: OAuthService,
              public signalRService: SignalRService) {
    // Remember the selected configuration
    this.configureImplicitFlow();

    // Automatically load user profile
    this.oauthService.events
      .pipe(filter(e => e.type === 'token_received'))
      .subscribe(_ => {
        console.log('state', this.oauthService.state);
        this.oauthService.loadUserProfile();
      });
  }

  private configureImplicitFlow() {
    this.oauthService.configure(authConfig);
    // this.oauthService.setStorage(localStorage);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();

    this.oauthService.loadDiscoveryDocumentAndTryLogin().then(_ => {
      this.router.navigate(['/']);
    });

    // Optional
    this.oauthService.setupAutomaticSilentRefresh();

    // Display all events
    this.oauthService.events.subscribe(e => {
      // tslint:disable-next-line:no-console
      console.debug('oauth/oidc event', e);
    });

    this.oauthService.events
      .pipe(filter(e => e.type === 'session_terminated'))
      .subscribe(e => {
        // tslint:disable-next-line:no-console
        console.debug('Your session has been terminated!');
      });
  }
}
