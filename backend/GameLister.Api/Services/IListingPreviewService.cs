using GameLister.Api.Dtos;

namespace GameLister.Api.Services;

public interface IListingPreviewService
{
    Task<ListingPreviewDto?> GetPreviewAsync(
        int listingDraftId,
        CancellationToken cancellationToken = default);
}
