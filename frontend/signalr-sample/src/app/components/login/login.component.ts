import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { authConfig } from '../../auth.config';

@Component({
  selector: 'sr-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {

  isLoading = false;
  userProfile: object;
  usePopup: boolean;
  login: false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private oauthService: OAuthService) {
  }

  ngOnInit() {
    this.isLoading = true;
    setTimeout(() => {
      if (this.givenName) {
        this.router.navigate(['home']).then(() => this.isLoading = false);
      }
      this.isLoading = false;
    }, 500);
  }

  async loginImplicit() {
    // Tweak config for implicit flow
    this.oauthService.configure(authConfig);
    await this.oauthService.loadDiscoveryDocument();
    sessionStorage.setItem('flow', 'implicit');
    this.oauthService.requestAccessToken = true;
    localStorage.setItem('requestAccessToken', '' + true);
    this.oauthService.initLoginFlow();
    // the parameter here is optional. It's passed around and can be used after logging in
  }

  logout() {
    // this.oauthService.logOut();
    this.oauthService.revokeTokenAndLogout();
  }

  loadUserProfile(): void {
    this.oauthService.loadUserProfile().then(up => (this.userProfile = up));
  }

  get givenName() {
    const claims = this.oauthService.getIdentityClaims();
    if (!claims) return null;
    return claims['name'];
  }

  refresh() {
    this.oauthService.oidc = true;

    if (
      !this.oauthService.useSilentRefresh &&
      this.oauthService.responseType === 'code'
    ) {
      this.oauthService
        .refreshToken()
        .then(info => console.log('refresh ok', info))
        .catch(err => console.error('refresh error', err));
    } else {
      this.oauthService
        .silentRefresh()
        .then(info => console.log('silent refresh ok', info))
        .catch(err => console.error('silent refresh error', err));
    }
  }

  submit(userName: string) {
    /*this.signalRService.registerUser(userName).subscribe(user => {
      console.log('Registered user', user);
      this.signalRService.currentUser = user;
      this.router.navigate(['chat']).catch(err => console.log(err));
    }, err => console.log(err));*/
  }

}
