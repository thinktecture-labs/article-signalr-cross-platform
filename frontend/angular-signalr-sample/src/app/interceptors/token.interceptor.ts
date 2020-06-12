import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Observable } from 'rxjs';

// REVIEW: den brauchst Du eigentlich nicht, da das die OIDC Lib vom Manfred auch kann über Config.
@Injectable()
export class TokenInterceptor implements HttpInterceptor {
  constructor(public auth: OAuthService) {}
  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

    request = request.clone({
      setHeaders: {
        Authorization: `Bearer ${this.auth.getAccessToken()}`
      }
    });
    return next.handle(request);
  }
}
