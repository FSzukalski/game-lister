using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GameLister.Api.Data;
using GameLister.Api.Dtos;
using GameLister.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameLister.Api.Services;

public interface IListingPreviewService
{
    Task<ListingPreviewDto?> GetPreviewAsync(int listingDraftId, CancellationToken cancellationToken = default);
}

public class ListingPreviewService : IListingPreviewService
{
    private readonly GameListerDbContext _db;

    public ListingPreviewService(GameListerDbContext db)
    {
        _db = db;
    }

    public async Task<ListingPreviewDto?> GetPreviewAsync(
        int listingDraftId,
        CancellationToken cancellationToken = default)
    {
        var listingDraft = await _db.ListingDrafts
            .AsNoTracking()
            .Include(ld => ld.Game)
            .FirstOrDefaultAsync(ld => ld.Id == listingDraftId, cancellationToken);

        if (listingDraft is null)
        {
            return null;
        }

        var game = listingDraft.Game;

        // --- opis HTML – bazowy, potem do rozbudowy ---
        var sb = new StringBuilder();

        sb.Append("<p>Sprzedam <strong>")
          .Append(game.Title);

        if (!string.IsNullOrWhiteSpace(game.Platform))
        {
            sb.Append("</strong> na <strong>")
              .Append(game.Platform);
        }

        sb.Append("</strong>.</p>");

        // priorytet: custom opis z draftu, potem opis z Game
        if (!string.IsNullOrWhiteSpace(listingDraft.DescriptionHtml))
        {
            sb.Append(listingDraft.DescriptionHtml);
        }
        else if (!string.IsNullOrWhiteSpace(game.Description))
        {
            sb.Append("<p>")
              .Append(game.Description)
              .Append("</p>");
        }

        // dodatkowe info (stan, kompletność)
        var details = new List<string>();

        if (!string.IsNullOrWhiteSpace(game.Condition))
        {
            details.Add($"Stan: {game.Condition}");
        }

        if (game.IsBoxIncluded)
        {
            details.Add("W zestawie: pudełko");
        }

        if (game.IsManualIncluded)
        {
            details.Add("W zestawie: instrukcja");
        }

        if (details.Count > 0)
        {
            sb.Append("<ul>");
            foreach (var d in details)
            {
                sb.Append("<li>")
                  .Append(d)
                  .Append("</li>");
            }
            sb.Append("</ul>");
        }

        var descriptionHtml = sb.ToString();

        // --- zdjęcia gry ---
        var images = await _db.GameImages
            .AsNoTracking()
            .Where(i => i.GameId == game.Id)
            .OrderByDescending(i => i.IsPrimary)
            .ThenBy(i => i.SortOrder)
            .ThenBy(i => i.Id)
            .ToListAsync(cancellationToken);

        var imageDtos = images
            .Select(i => new GameImageDto(
                i.Id,
                       i.Url,
                i.Type.ToString(),
                i.SortOrder,
                i.IsPrimary))
            .ToList()
            .AsReadOnly();

        var priceDto = new MoneyDto(listingDraft.Price.Amount, listingDraft.Price.Currency);

        return new ListingPreviewDto(
            listingDraft.Id,
            listingDraft.GameId,
            listingDraft.Marketplace,
            listingDraft.Title,
            listingDraft.Subtitle,
            descriptionHtml,
            priceDto,
            DateTime.UtcNow,
            imageDtos
        );
    }
}
