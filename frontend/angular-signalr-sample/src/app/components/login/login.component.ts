import { Component } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'sr-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent {
  isLoading = false;

  constructor(
    private oauthService: OAuthService) {
  }

  public async loginCode(): Promise<void> {
    // Tweak config for code flow
    await this.oauthService.loadDiscoveryDocument();
    sessionStorage.setItem('flow', 'code');
    this.oauthService.initLoginFlow();
  }
}
