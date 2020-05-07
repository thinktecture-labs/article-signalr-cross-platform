// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using System.Collections.Generic;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;

namespace PushSample.IdentityServer
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
                // new ApiResource("push-api", "Push API", )
                new ApiResource
                {
                    Name = "push-api",
                    DisplayName = "Push API",
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
                            Name = "push-api.full_access",
                            DisplayName = "Full access to API 2",
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
                    ClientId = "push-spa",
                    ClientName = "Angular Push SPA",
                    AllowedGrantTypes = GrantTypes.Code,
                    ClientSecrets =
                    {
                        new Secret("pwasecret".Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "push-api.full_access",
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
                    ClientId = "blazor-spa",
                    ClientName = "Blazor SPA",
                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "push-api.full_access",
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
                        new Secret("thisismyclientspecificsecret".Sha256())
                    },
                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "push-api.full_access",
                        "custom.profile"
                    }
                },
            };
    }
}