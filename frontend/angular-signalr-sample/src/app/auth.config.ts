import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from '../environments/environment';

export const authConfig: AuthConfig = {
  // Url of the Identity Provider
  issuer: environment.identityBaseUrl,
  redirectUri: window.location.origin + '/callback',
  // The SPA's id. The SPA is registerd with this id at the auth-server
  clientId: 'crossapp',
  responseType: 'code',
  dummyClientSecret: 'crossapp-secret',
  // REVIEW: Hier passt der Kommentar nicht mehr zum Code drunter. CHECK
  // set the scope for the permissions the client should request
  // The first three are defined by OIDC. The 4th is a usecase-specific one
  scope: 'openid profile offline_access signalr-api.full_access',
  showDebugInformation: true,
  requireHttps: false,
  useSilentRefresh: false,
  timeoutFactor: 0.01,
  clearHashAfterLogin: false
};
