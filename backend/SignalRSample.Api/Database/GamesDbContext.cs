using Microsoft.EntityFrameworkCore;
using SignalRSample.Api.Models;

namespace SignalRSample.Api.Database
{
    public class GamesDbContext : DbContext
    {
        public DbSet<GameSession> Sessions { get; set; }
        public DbSet<GameSessionMove> GameSessionMoves { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<User> Users { get; set; }
        
        public GamesDbContext(DbContextOptions<GamesDbContext> options)
            : base(options)
        {
        }
    }
}