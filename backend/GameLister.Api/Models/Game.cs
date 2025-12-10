namespace GameLister.Api.Models;

public class Game
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Platform { get; set; }
    public string? Condition { get; set; }
    public decimal? Price { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
