using System.Text;
using GameLister.Api.Data;
using GameLister.Api.Dtos;
using Microsoft.EntityFrameworkCore;

namespace GameLister.Api.Services;

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
        var draft = await _db.ListingDrafts
            .Include(ld => ld.Game)
            .FirstOrDefaultAsync(ld => ld.Id == listingDraftId, cancellationToken);

        if (draft is null || draft.Game is null)
            return null;

        var descriptionHtml = BuildHtmlDescription(draft.Game, draft);

        return new ListingPreviewDto(
            ListingDraftId: draft.Id,
            GameId: draft.GameId,
            Marketplace: draft.Marketplace,
            Title: draft.Title,
            Subtitle: draft.Subtitle,
            DescriptionHtml: descriptionHtml,
            Price: new MoneyDto(draft.Price.Amount, draft.Price.Currency),
            GeneratedAtUtc: DateTime.UtcNow
        );
    }

    private static string BuildHtmlDescription(Models.Game game, Models.ListingDraft draft)
    {
        var sb = new StringBuilder();

        sb.Append("<p><strong>")
          .Append(System.Net.WebUtility.HtmlEncode(game.Title));

        if (!string.IsNullOrWhiteSpace(game.Platform))
        {
            sb.Append(" (")
              .Append(System.Net.WebUtility.HtmlEncode(game.Platform))
              .Append(')');
        }

        sb.Append("</strong></p>");

        if (!string.IsNullOrWhiteSpace(draft.DescriptionHtml))
        {
            sb.Append(draft.DescriptionHtml);
        }
        else if (!string.IsNullOrWhiteSpace(game.Description))
        {
            sb.Append("<p>")
              .Append(System.Net.WebUtility.HtmlEncode(game.Description))
              .Append("</p>");
        }

        sb.Append("<ul>");

        if (!string.IsNullOrWhiteSpace(game.Condition))
        {
            sb.Append("<li>Stan: ")
              .Append(System.Net.WebUtility.HtmlEncode(game.Condition))
              .Append("</li>");
        }

        if (!string.IsNullOrWhiteSpace(game.Language))
        {
            sb.Append("<li>Język: ")
              .Append(System.Net.WebUtility.HtmlEncode(game.Language))
              .Append("</li>");
        }

        if (!string.IsNullOrWhiteSpace(game.Region))
        {
            sb.Append("<li>Region: ")
              .Append(System.Net.WebUtility.HtmlEncode(game.Region))
              .Append("</li>");
        }

        sb.Append("</ul>");

        return sb.ToString();
    }
}
