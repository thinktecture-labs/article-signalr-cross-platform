using System;
using Microsoft.AspNetCore.Identity;

namespace SignalRSample.IdentityServer.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsAdmin { get; set; }
        public string AvatarUrl { get; set; }
        public DateTime AccountExpires { get; set; }
    }
}