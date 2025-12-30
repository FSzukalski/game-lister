namespace GameLister.Api.Models;

public enum GameImageType
{
    Unknown = 0,
    FrontCover = 1,
    BackCover = 2,
    DiscOrCartridge = 3,
    Manual = 4,
    BoxSet = 5,
    InGameScreenshot = 6,
    Other = 99
}

public class GameImage
{
    public int Id { get; set; }

    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    public string Url { get; set; } = null!;
    public GameImageType Type { get; set; } = GameImageType.Unknown;

    public int SortOrder { get; set; } = 0;
    public bool IsPrimary { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Jedna, jednoznaczna nawigacja
    public ICollection<ListingDraftImage> ListingDraftImages { get; set; } = new List<ListingDraftImage>();
}
