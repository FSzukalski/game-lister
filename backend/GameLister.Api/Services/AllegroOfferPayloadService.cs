using GameLister.Api.Data;
using GameLister.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameLister.Api.Services;

public interface IAllegroOfferPayloadService
{
    Task<AllegroOfferPayloadDto?> BuildAllegroOfferPayloadAsync(
        int listingDraftId,
        CancellationToken cancellationToken = default);
}

public class AllegroOfferPayloadService : IAllegroOfferPayloadService
{
    private readonly GameListerDbContext _dbContext;
    private readonly IListingPreviewService _listingPreviewService;

    public AllegroOfferPayloadService(
        GameListerDbContext dbContext,
        IListingPreviewService listingPreviewService)
    {
        _dbContext = dbContext;
        _listingPreviewService = listingPreviewService;
    }

    public async Task<AllegroOfferPayloadDto?> BuildAllegroOfferPayloadAsync(
        int listingDraftId,
        CancellationToken cancellationToken = default)
    {
        var draft = await _dbContext.ListingDrafts
            .AsNoTracking()
            .FirstOrDefaultAsync(ld => ld.Id == listingDraftId, cancellationToken);

        if (draft is null)
            return null;

        var preview = await _listingPreviewService.GetPreviewAsync(listingDraftId, cancellationToken);
        if (preview is null)
            return null;

        // 1) Najpierw bierzemy zdjęcia przypięte do draftu
        var selected = await _dbContext.ListingDraftImages
            .AsNoTracking()
            .Where(x => x.ListingDraftId == listingDraftId)
            .Include(x => x.GameImage)
            .OrderBy(x => x.SortOrder)
            .Select(x => x.GameImage.Url)
            .ToListAsync(cancellationToken);

        // 2) Jeśli nie ma selekcji w drafcie -> fallback: zdjęcia gry
        var imageUrls = selected;

        if (imageUrls.Count == 0)
        {
            imageUrls = await _dbContext.GameImages
                .AsNoTracking()
                .Where(i => i.GameId == draft.GameId)
                .OrderByDescending(i => i.IsPrimary)
                .ThenBy(i => i.SortOrder)
                .ThenBy(i => i.Id)
                .Select(i => i.Url)
                .ToListAsync(cancellationToken);
        }

        imageUrls = imageUrls
            .Where(u => !string.IsNullOrWhiteSpace(u))
            .ToList();

        return new AllegroOfferPayloadDto(
            Title: preview.Title,
            DescriptionHtml: preview.DescriptionHtml,
            CategoryId: draft.CategoryId,
            Language: draft.Language,
            Price: preview.Price,
            ImageUrls: imageUrls
        );
    }
}
