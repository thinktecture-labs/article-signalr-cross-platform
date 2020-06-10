using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace SignalRSample.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var customProfile = new IdentityResource(
                "custom.profile",
                "Custom profile",
                new[]
                {
                    JwtClaimTypes.Name,
                    JwtClaimTypes.Subject,
                    JwtClaimTypes.WebSite,
                    JwtClaimTypes.Email
                });

            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                customProfile
            };
        }

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
                        JwtClaimTypes.WebSite,
                        JwtClaimTypes.Email
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
                        "signalr-api.full_access",
                        "custom.profile"
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
                    ClientSecrets =
                    {
                        new Secret("blazor-spa-secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "signalr-api.full_access",
                        "custom.profile"
                    },
                    RedirectUris =
                    {
                        "https://localhost:6001/oidc/callbacks/authentication-redirect",
                        "https://localhost:6001/_content/Sotsera.Blazor.Oidc/authentication-popup.html",
                        "http://localhost:52310/oidc/callbacks/authentication-redirect",
                        "http://localhost:52310/_content/Sotsera.Blazor.Oidc/authentication-popup.html"
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
}