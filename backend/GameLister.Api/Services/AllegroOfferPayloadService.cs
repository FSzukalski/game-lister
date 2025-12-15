using System.Globalization;
using GameLister.Api.Dtos;

namespace GameLister.Api.Services;

public interface IAllegroOfferPayloadService
{
    Task<object?> BuildAllegroOfferPayloadAsync(
        int listingDraftId,
        CancellationToken cancellationToken = default);
}

public class AllegroOfferPayloadService : IAllegroOfferPayloadService
{
    private readonly IListingPreviewService _previewService;

    public AllegroOfferPayloadService(IListingPreviewService previewService)
    {
        _previewService = previewService;
    }

    public async Task<object?> BuildAllegroOfferPayloadAsync(
        int listingDraftId,
        CancellationToken cancellationToken = default)
    {
        var preview = await _previewService.GetPreviewAsync(listingDraftId, cancellationToken);
        if (preview is null)
            return null;

        var payload = new
        {
            name = preview.Title,
            sellingMode = new
            {
                format = "BUY_NOW",
                price = new
                {
                    amount = preview.Price.Amount.ToString("0.00", CultureInfo.InvariantCulture),
                    currency = preview.Price.Currency
                }
            },
            description = new
            {
                sections = new[]
                {
                    new
                    {
                        items = new[]
                        {
                            new
                            {
                                type = "TEXT",
                                content = preview.DescriptionHtml
                            }
                        }
                    }
                }
            }
            // TODO: category, images, shipping, etc.
        };

        return payload;
    }
}
