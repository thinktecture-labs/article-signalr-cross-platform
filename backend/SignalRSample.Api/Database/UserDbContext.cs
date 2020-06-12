using Microsoft.EntityFrameworkCore;
using SignalRSample.Api.Models;

namespace SignalRSample.Api.Database
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        
        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }
    }
}