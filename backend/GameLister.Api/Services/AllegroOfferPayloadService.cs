using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        // 1. Pobierz draft
        var draft = await _dbContext.ListingDrafts
            .AsNoTracking()
            .FirstOrDefaultAsync(ld => ld.Id == listingDraftId, cancellationToken);

        if (draft is null)
        {
            return null;
        }

        // 2. Pobierz preview (u nas: gotowy tytuł, opis, cena)
        var preview = await _listingPreviewService.GetPreviewAsync(listingDraftId, cancellationToken);
        if (preview is null)
        {
            return null;
        }

        // 3. Pobierz obrazy dla gry
        var images = await _dbContext.GameImages
            .AsNoTracking()
            .Where(i => i.GameId == draft.GameId)
            .OrderBy(i => i.SortOrder)
            .ToListAsync(cancellationToken);

        var imageUrls = images
            .Select(i => i.Url)
            .Where(u => !string.IsNullOrWhiteSpace(u))
            .ToList();

        // 4. Złóż payload „pod Allegro”
        var payload = new AllegroOfferPayloadDto(
            Title: preview.Title,
            DescriptionHtml: preview.DescriptionHtml,
            CategoryId: draft.CategoryId,
            Language: draft.Language,
            Price: preview.Price,
            ImageUrls: imageUrls
        );

        return payload;
    }
}
