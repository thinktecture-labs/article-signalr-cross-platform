// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


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

        public static IEnumerable<ApiScope> ApiScopes =>
            new[]
            {
                new ApiScope("signalr-api.full_access", "Full access to SignalR API"),
                new ApiScope("messenger-api.full_access", "Full access to Messenger API")
            };

        public static IEnumerable<ApiResource> Apis =>
            new[]
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
                    Scopes = {"signalr-api.full_access"}
                },
                new ApiResource
                {
                    Name = "messenger-api",
                    DisplayName = "Messenger API",
                    UserClaims =
                    {
                        JwtClaimTypes.Name,
                        JwtClaimTypes.Subject,
                        JwtClaimTypes.Role
                    },
                    Scopes = {"messenger-api.full_access"}
                },
            };

        public static IEnumerable<Client> Clients =>
            new[]
            {
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
                    AccessTokenLifetime = 31556952
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
                    AccessTokenLifetime = 31556952
                },
                new Client
                {
                    RequireConsent = false,
                    ClientId = "music-messenger-app",
                    ClientName = "Music Messenger PWA",
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowOfflineAccess = true,
                    ClientSecrets =
                    {
                        new Secret("music-messenger-app-secret".Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "messenger-api.full_access"
                    },
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:4200/#/login",
                        "http://127.0.0.1:2020/#/login",
                        "http://192.168.0.106:2020/#/login",
                        "https://music-messenger.azurewebsites.net/#/login"
                    },
                    RedirectUris =
                    {
                        "http://localhost:4200/#/callback",
                        "http://127.0.0.1:2020/#/callback",
                        "http://192.168.0.106:2020/#/callback",
                        "https://music-messenger.azurewebsites.net/#/callback"
                    },
                    AllowedCorsOrigins =
                    {
                        "http://localhost:4200",
                        "http://127.0.0.1:2020",
                        "http://192.168.0.106:2020",
                        "https://music-messenger.azurewebsites.net"
                    },
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenLifetime = 31556952
                },
            };
    }

    public class ProfileWithRoleIdentityResource
        : IdentityResources.Profile
    {
        public ProfileWithRoleIdentityResource()
        {
            UserClaims.Add(JwtClaimTypes.Name);
            UserClaims.Add(JwtClaimTypes.Subject);
            UserClaims.Add(JwtClaimTypes.WebSite);
            UserClaims.Add(JwtClaimTypes.Email);
            UserClaims.Add(JwtClaimTypes.Role);
        }
    }
}