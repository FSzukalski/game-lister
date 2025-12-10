using Microsoft.EntityFrameworkCore;
using GameLister.Api.Models;

namespace GameLister.Api.Data;

public class GameListerDbContext : DbContext
{
    public GameListerDbContext(DbContextOptions<GameListerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Game> Games => Set<Game>();
}
