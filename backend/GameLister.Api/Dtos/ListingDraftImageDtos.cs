namespace GameLister.Api.Dtos;

public record ListingDraftImageDto(
    int GameImageId,
    string Url,
    string Type,
    int SortOrder,
    bool IsPrimary
);

public class ListingDraftImageUpsertItemDto
{
    public int GameImageId { get; set; }
    public int SortOrder { get; set; }
}

public class ListingDraftImagesUpsertDto
{
    public List<ListingDraftImageUpsertItemDto> Items { get; set; } = new();
}
