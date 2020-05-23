import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthenticatedGuard implements CanActivate, CanActivateChild {

  constructor(private readonly oAuthService: OAuthService,
              private readonly router: Router) {
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.internalCanActivate(state.url);
  }

  canActivateChild(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.internalCanActivate(state.url);
  }

  private internalCanActivate(redirectUri: string): Observable<boolean | UrlTree> {
    return new Observable<boolean | UrlTree>((observer) => {
      if (this.givenName) {
        observer.next(true);
      }

      observer.next(this.router.createUrlTree(['/login']));
    });
  }

  private get givenName() {
    const claims = this.oAuthService.getIdentityClaims();
    if (!claims) {
      return null;
    }
    return claims['name'];
  }
}
