namespace GameLister.Api.Dtos;

public record ListingDraftImageDto(
    int GameImageId,
    string Url,
    string Type,
    int SortOrder,
    bool IsPrimary
);

public record ListingDraftImageUpsertItemDto(
    int GameImageId,
    int SortOrder
);

public class ListingDraftImagesUpsertDto
{
    public List<ListingDraftImageUpsertItemDto> Items { get; set; } = new();
}
