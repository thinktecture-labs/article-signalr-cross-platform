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

        //TODO: Relationship!
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>().HasOne(m => m.UserOne)
                .WithMany(m => m.UserOneGames).HasForeignKey(m => m.UserOneId);
            modelBuilder.Entity<Game>().HasOne(m => m.UserTwo)
                .WithMany(m => m.UserTwoGames).HasForeignKey(m => m.UserTwoId);

            /*modelBuilder.Entity<Game>()
                .HasMany(g => g.Users)
                .WithOne()
                .HasForeignKey(nameof(Game.UserOneId))
                .IsRequired();
            modelBuilder.Entity<Game>()
                .HasMany(g => g.Users)
                .WithOne()
                .HasForeignKey(nameof(Game.UserTwoId))
                .IsRequired(false);*/
            base.OnModelCreating(modelBuilder);
        }
    }
}