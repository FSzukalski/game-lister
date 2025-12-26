using System;
using System.Collections.Generic;

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

    // FK do Game
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    /// <summary>
    /// Publiczny URL obrazka (docelowo np. z naszego storage / CDN)
    /// </summary>
    public string Url { get; set; } = null!;

    /// <summary>
    /// Typ zdjęcia – przód, tył, płyta, instrukcja itd.
    /// </summary>
    public GameImageType Type { get; set; } = GameImageType.Unknown;

    /// <summary>
    /// Kolejność wyświetlania (0 = pierwsze).
    /// </summary>
    public int SortOrder { get; set; } = 0;

    /// <summary>
    /// Czy to zdjęcie główne (miniaturka oferty).
    /// </summary>
    public bool IsPrimary { get; set; } = false;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<ListingDraftImage> ListingDraftLinks { get; set; } = new List<ListingDraftImage>();
    public ICollection<ListingDraftImage> ListingDraftImages { get; set; } = new List<ListingDraftImage>();

}
