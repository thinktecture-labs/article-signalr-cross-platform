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
                    ClientId = "angular-spa",
                    ClientName = "Angular Push SPA",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets =
                    {
                        new Secret("angular-spa-secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "signalr-api.full_access",
                        "custom.profile"
                    },
                    RedirectUris = {"http://localhost:4200/login"},
                    PostLogoutRedirectUris = {"http://localhost:4200/home"},
                    AllowedCorsOrigins = {"http://localhost:4200"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                },
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
                    ClientSecrets =
                    {
                        new Secret("blazor-spa-secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "signalr-api.full_access",
                        "custom.profile"
                    },
                    RedirectUris = {"https://localhost:6001/oidc/callbacks/authentication-redirect"},
                    PostLogoutRedirectUris = {"https://localhost:6001/"},
                    AllowedCorsOrigins = {"https://localhost:6001"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 3600
                },
                new Client
                {
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientId = "blazor-server",
                    ClientSecrets =
                    {
                        new Secret("blazor-server-secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "signalr-api.full_access",
                        "custom.profile"
                    }
                },
            };
    }
}