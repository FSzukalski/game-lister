namespace GameLister.Api.Models;

public class GameImage
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public string Url { get; set; } = string.Empty;   // ścieżka lokalna / URL
    public int Order { get; set; }                    // kolejność zdjęcia w aukcji
    public string? Label { get; set; }                // np. "Front", "Tył", "Płyta"
}
