using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace SignalRSample.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
                new IdentityResources.OpenId(),

                // let's include the role claim in the profile
                new ProfileWithRoleIdentityResource(),
                new IdentityResources.Email()
            };

        public static IEnumerable<ApiResource> Apis =>
            new ApiResource[]
            {
                new ApiResource
                {
                    Name = "signalr-api",
                    DisplayName = "Sample API",
                    UserClaims =
                    {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Subject,
                        JwtClaimTypes.Role
                    },
                    Scopes =
                    {
                        new Scope()
                        {
                            Name = "signalr-api.full_access",
                            DisplayName = "Full access to SignalR API",
                        }
                    }
                },
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client
                {
                    RequireConsent = false,
                    ClientId = "crossapp",
                    ClientName = "Cross Plattform Sample",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowOfflineAccess = true,
                    ClientSecrets =
                    {
                        new Secret("crossapp-secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "signalr-api.full_access"
                    },
                    PostLogoutRedirectUris =
                    {
                        "tictactoe://localhost/home",
                        "http://localhost:4200/home"
                    },
                    RedirectUris =
                    {
                        "tictactoe://localhost/callback",
                        "http://localhost:4200/callback"
                    },
                    AllowedCorsOrigins =
                    {
                        "tictactoe://localhost",
                        "http://localhost:4200"
                    },
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                },
                new Client
                {
                    RequireConsent = false,
                    ClientId = "blazor-spa",
                    ClientName = "Blazor SPA",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowOfflineAccess = true,
                    RequireClientSecret = false,
                    ClientSecrets =
                    {
                        new Secret("blazor-spa-secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "signalr-api.full_access"
                    },
                    RedirectUris =
                    {
                        "https://localhost:6001/oidc/callbacks/authentication-redirect",
                        "https://localhost:6001/authentication/login-callback",
                    },
                    PostLogoutRedirectUris =
                    {
                        "https://localhost:6001/",
                        "http://localhost:52310/"
                    },
                    AllowedCorsOrigins =
                    {
                        "https://localhost:6001",
                        "http://localhost:52310"
                    },
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                }
            };
    }

    public class ProfileWithRoleIdentityResource
        : IdentityResources.Profile
    {
        public ProfileWithRoleIdentityResource()
        {
            this.UserClaims.Add(JwtClaimTypes.Name);
            this.UserClaims.Add(JwtClaimTypes.Subject);
            this.UserClaims.Add(JwtClaimTypes.WebSite);
            this.UserClaims.Add(JwtClaimTypes.Email);
            this.UserClaims.Add(JwtClaimTypes.Role);
        }
    }
}