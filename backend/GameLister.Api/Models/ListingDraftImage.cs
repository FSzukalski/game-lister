using System;

namespace GameLister.Api.Models;

public class ListingDraftImage
{
    public int Id { get; set; }

    public int ListingDraftId { get; set; }
    public ListingDraft ListingDraft { get; set; } = null!;

    public int GameImageId { get; set; }
    public GameImage GameImage { get; set; } = null!;

    public int SortOrder { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
