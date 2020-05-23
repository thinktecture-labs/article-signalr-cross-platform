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
        
        public static string UserId(this ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst(c => c.Type == "sub");
            return userIdClaim != null ? userIdClaim.Value : String.Empty;
        }
    }
}