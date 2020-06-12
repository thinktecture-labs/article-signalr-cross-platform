import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';

@Component({
  selector: 'sr-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.scss'],
})
export class CallbackComponent implements OnInit {

  constructor(private readonly oauthService: OAuthService, private readonly router: Router) {
  }

  public ngOnInit(): void {
    this.oauthService.loadDiscoveryDocumentAndTryLogin().then(success => {
        console.log('loadDiscoveryDocumentAndTryLogin', success);
        this.router.navigate(['/']);
      },
      _ => this.router.navigate(['login']),
    );
  }
}
