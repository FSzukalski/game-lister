using GameLister.Api.Models;
using GameLister.Api.Models.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace GameLister.Api.Data;

public class GameListerDbContext : DbContext
{
    public DbSet<Game> Games => Set<Game>();
    public DbSet<GameImage> GameImages => Set<GameImage>();

    public GameListerDbContext(DbContextOptions<GameListerDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Value object Money jako owned type
        modelBuilder.Entity<Game>()
            .OwnsOne(g => g.Price);

        // Relacja 1-wiele Game → GameImage
        modelBuilder.Entity<GameImage>()
            .HasOne(gi => gi.Game)
            .WithMany(g => g.Images)
            .HasForeignKey(gi => gi.GameId);
    }
}
