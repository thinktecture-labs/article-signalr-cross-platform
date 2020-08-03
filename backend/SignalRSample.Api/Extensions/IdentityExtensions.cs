using System;
using System.Security.Claims;
using IdentityModel;

namespace SignalRSample.Api.Extensions
{
    public static class IdentityExtensions
    {
        public static string UserName(this ClaimsPrincipal user)
        {
            var userNameClaim = user.FindFirst(c => c.Type == JwtClaimTypes.Name);
            return userNameClaim != null ? userNameClaim.Value : String.Empty;
        }

        public static string SubId(this ClaimsPrincipal user)
        {
            var userNameClaim = user.FindFirst(c => c.Type == JwtClaimTypes.Subject);
            return userNameClaim != null ? userNameClaim.Value : String.Empty;
        }
    }
}