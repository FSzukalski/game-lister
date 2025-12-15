using GameLister.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLister.Api.Data;

public class GameListerDbContext : DbContext
{
    public GameListerDbContext(DbContextOptions<GameListerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();
    public DbSet<ListingDraft> ListingDrafts => Set<ListingDraft>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ------- Game -------
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(g => g.Id);

            entity.Property(g => g.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(g => g.Subtitle)
                .HasMaxLength(200);

            entity.Property(g => g.Platform)
                .HasMaxLength(100);

            entity.Property(g => g.Edition)
                .HasMaxLength(100);

            entity.Property(g => g.Language)
                .HasMaxLength(50);

            entity.Property(g => g.Region)
                .HasMaxLength(50);

            entity.Property(g => g.Condition)
                .HasMaxLength(50);

            // Money jako owned type – jawny typ kolumny
            entity.OwnsOne(g => g.Price, money =>
            {
                money.Property(m => m.Amount)
                     .HasColumnName("Price_Amount")
                     .HasColumnType("decimal(18,2)");

                money.Property(m => m.Currency)
                     .HasColumnName("Price_Currency")
                     .HasMaxLength(3);
            });

            entity.HasMany(g => g.ListingDrafts)
                  .WithOne(ld => ld.Game)
                  .HasForeignKey(ld => ld.GameId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ------- ListingDraft -------
        modelBuilder.Entity<ListingDraft>(entity =>
        {
            entity.HasKey(ld => ld.Id);

            entity.Property(ld => ld.Marketplace)
                  .IsRequired()
                  .HasMaxLength(50);

            entity.Property(ld => ld.Title)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.Property(ld => ld.Subtitle)
                  .HasMaxLength(200);

            entity.Property(ld => ld.CategoryId)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(ld => ld.Language)
                  .IsRequired()
                  .HasMaxLength(10);

            entity.Property(ld => ld.Status)
                  .IsRequired()
                  .HasMaxLength(30);

            entity.OwnsOne(ld => ld.Price, money =>
            {
                money.Property(m => m.Amount)
                     .HasColumnName("Price_Amount")
                     .HasColumnType("decimal(18,2)");

                money.Property(m => m.Currency)
                     .HasColumnName("Price_Currency")
                     .HasMaxLength(3);
            });
        });
    }
}
