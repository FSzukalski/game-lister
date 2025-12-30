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
    public DbSet<GameImage> GameImages => Set<GameImage>();
    public DbSet<ListingDraftImage> ListingDraftImages => Set<ListingDraftImage>();

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

            entity.Property(g => g.Subtitle).HasMaxLength(200);
            entity.Property(g => g.Platform).HasMaxLength(100);
            entity.Property(g => g.Edition).HasMaxLength(100);
            entity.Property(g => g.Language).HasMaxLength(50);
            entity.Property(g => g.Region).HasMaxLength(50);
            entity.Property(g => g.Condition).HasMaxLength(50);

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

            entity.HasMany(g => g.Images)
                  .WithOne(i => i.Game)
                  .HasForeignKey(i => i.GameId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ------- ListingDraft -------
        modelBuilder.Entity<ListingDraft>(entity =>
        {
            entity.HasKey(ld => ld.Id);

            entity.Property(ld => ld.Marketplace).IsRequired().HasMaxLength(50);
            entity.Property(ld => ld.Title).IsRequired().HasMaxLength(200);
            entity.Property(ld => ld.Subtitle).HasMaxLength(200);

            entity.Property(ld => ld.CategoryId).IsRequired().HasMaxLength(100);
            entity.Property(ld => ld.Language).IsRequired().HasMaxLength(10);
            entity.Property(ld => ld.Status).IsRequired().HasMaxLength(30);

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

        // ------- GameImage -------
        modelBuilder.Entity<GameImage>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.Url)
                  .IsRequired()
                  .HasMaxLength(500);

            entity.Property(i => i.Type)
                  .HasConversion<int>();

            entity.Property(i => i.SortOrder)
                  .HasDefaultValue(0);

            entity.Property(i => i.IsPrimary)
                  .HasDefaultValue(false);

            entity.Property(i => i.CreatedAt)
                  .IsRequired();
        });

        // ------- ListingDraftImage -------
        modelBuilder.Entity<ListingDraftImage>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.HasIndex(x => new { x.ListingDraftId, x.GameImageId })
                  .IsUnique();

            entity.Property(x => x.SortOrder)
                  .HasDefaultValue(0);

            entity.Property(x => x.CreatedAt)
                  .IsRequired();

            entity.HasOne(x => x.ListingDraft)
                  .WithMany(d => d.Images)
                  .HasForeignKey(x => x.ListingDraftId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.GameImage)
                  .WithMany(i => i.ListingDraftImages)
                  .HasForeignKey(x => x.GameImageId)
                  .OnDelete(DeleteBehavior.Cascade);
        });


    }
}
