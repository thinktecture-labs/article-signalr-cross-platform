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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasOne(m => m.UserOne)
                .WithMany(m => m.UserOneGames).HasForeignKey(m => m.UserOneId);
            modelBuilder.Entity<Game>().HasOne(m => m.UserTwo)
                .WithMany(m => m.UserTwoGames).HasForeignKey(m => m.UserTwoId);
            base.OnModelCreating(modelBuilder);
        }
    }
}