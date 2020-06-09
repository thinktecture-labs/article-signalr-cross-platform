import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from '../environments/environment';

export const authConfig: AuthConfig = {
  // Url of the Identity Provider
  issuer: environment.identityBaseUrl,
  redirectUri: window.location.origin + '/callback',
  // URL of the SPA to redirect the user after silent refresh
  silentRefreshRedirectUri: window.location.origin + '/silent-refresh.html',
  // The SPA's id. The SPA is registerd with this id at the auth-server
  clientId: 'crossapp',
  responseType: 'code',
  dummyClientSecret: 'crossapp-secret',
  // set the scope for the permissions the client should request
  // The first three are defined by OIDC. The 4th is a usecase-specific one
  scope: 'openid profile signalr-api.full_access offline_access',
  showDebugInformation: true,
  requireHttps: false,
  useSilentRefresh: false,
  timeoutFactor: 0.01,
  clearHashAfterLogin: false
};
