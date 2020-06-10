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
    return this.internalCanActivate();
  }

  canActivateChild(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    return this.internalCanActivate();
  }

  private internalCanActivate(): Observable<boolean | UrlTree> {
    return new Observable<boolean | UrlTree>((observer) => {
      if (this.oAuthService.getAccessToken()) {
        observer.next(true);
      }

      observer.next(this.router.createUrlTree(['/login']));
    });
  }
}
