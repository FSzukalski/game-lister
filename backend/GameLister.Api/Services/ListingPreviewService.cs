using System.Text;
using GameLister.Api.Dtos;
using GameLister.Api.Models;
using GameLister.Api.Models.ValueObjects;

namespace GameLister.Api.Services;

public interface IListingPreviewService
{
    ListingPreviewDto GeneratePreview(ListingDraft listing, Game game);
}

public class ListingPreviewService : IListingPreviewService
{
    public ListingPreviewDto GeneratePreview(ListingDraft listing, Game game)
    {
        var now = DateTime.UtcNow;

        // 1. Tytuł aukcji
        var title = string.IsNullOrWhiteSpace(listing.Title)
            ? BuildTitleFromGame(game)
            : listing.Title;

        // 2. Podtytuł
        var subtitle = listing.Subtitle;
        if (string.IsNullOrWhiteSpace(subtitle))
        {
            subtitle = BuildDefaultSubtitle(game);
        }

        // 3. Opis HTML – jeśli użytkownik już coś wpisał, to nie nadpisujemy,
        // tylko ewentualnie delikatnie wzbogacamy.
        var descriptionHtml = string.IsNullOrWhiteSpace(listing.DescriptionHtml)
            ? BuildDescriptionFromGame(listing, game)
            : listing.DescriptionHtml;

        // 4. Cena
        var priceDto = new MoneyDto(listing.Price.Amount, listing.Price.Currency);

        return new ListingPreviewDto(
            ListingDraftId: listing.Id,
            GameId: listing.GameId,
            Marketplace: listing.Marketplace,
            Title: title,
            Subtitle: subtitle,
            DescriptionHtml: descriptionHtml,
            Price: priceDto,
            GeneratedAtUtc: now
        );
    }

    private static string BuildTitleFromGame(Game game)
    {
        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(game.Title))
            parts.Add(game.Title);

        if (!string.IsNullOrWhiteSpace(game.Platform))
            parts.Add($"({game.Platform})");

        if (!string.IsNullOrWhiteSpace(game.Edition))
            parts.Add(game.Edition);

        if (!string.IsNullOrWhiteSpace(game.Condition))
            parts.Add($"- {game.Condition}");

        return string.Join(" ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
    }

    private static string BuildDefaultSubtitle(Game game)
    {
        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(game.Region))
            parts.Add(game.Region);

        if (!string.IsNullOrWhiteSpace(game.Language))
            parts.Add($"wersja {game.Language}");

        if (!string.IsNullOrWhiteSpace(game.Condition))
            parts.Add($"stan: {game.Condition}");

        var result = string.Join(", ", parts.Where(p => !string.IsNullOrWhiteSpace(p)));
        return string.IsNullOrWhiteSpace(result) ? "Aukcja gry" : result;
    }

    private static string BuildDescriptionFromGame(ListingDraft listing, Game game)
    {
        var sb = new StringBuilder();

        sb.AppendLine("<h2>Szczegóły gry</h2>");
        sb.Append("<p>");

        sb.Append(Escape(game.Title));

        if (!string.IsNullOrWhiteSpace(game.Platform))
            sb.Append($" na <strong>{Escape(game.Platform)}</strong>");

        if (!string.IsNullOrWhiteSpace(game.Edition))
            sb.Append($", edycja: {Escape(game.Edition)}");

        sb.AppendLine(".</p>");

        // Główne cechy
        sb.AppendLine("<h3>Stan i kompletność</h3>");
        sb.AppendLine("<ul>");

        if (!string.IsNullOrWhiteSpace(game.Condition))
            sb.AppendLine($"  <li>Stan: {Escape(game.Condition)}</li>");

        sb.AppendLine($"  <li>Oryginał: {(game.IsOriginal ? "tak" : "nie / niepewne")}</li>");
        sb.AppendLine($"  <li>Pudełko: {(game.IsBoxIncluded ? "w zestawie" : "brak")}</li>");
        sb.AppendLine($"  <li>Instrukcja: {(game.IsManualIncluded ? "w zestawie" : "brak")}</li>");

        if (!string.IsNullOrWhiteSpace(game.Language))
            sb.AppendLine($"  <li>Język: {Escape(game.Language)}</li>");

        if (!string.IsNullOrWhiteSpace(game.Region))
            sb.AppendLine($"  <li>Region: {Escape(game.Region)}</li>");

        sb.AppendLine("</ul>");

        // Opis użytkownika gry
        if (!string.IsNullOrWhiteSpace(game.Description))
        {
            sb.AppendLine("<h3>Dodatkowy opis</h3>");
            sb.AppendLine($"<p>{Escape(game.Description)}</p>");
        }

        // Info o cenie
        sb.AppendLine("<h3>Cena</h3>");
        sb.AppendLine($"<p>Cena wywoławcza: <strong>{listing.Price.Amount:0.00} {Escape(listing.Price.Currency)}</strong>.</p>");

        // Subtelna stopka o szkicu
        sb.AppendLine("<hr/>");
        sb.AppendLine("<p style=\"font-size: 0.9em; color: #666;\">Ten opis został wygenerowany automatycznie na podstawie danych o grze. Przed wystawieniem aukcji możesz go dowolnie edytować.</p>");

        return sb.ToString();
    }

    private static string Escape(string? input)
    {
        if (string.IsNullOrEmpty(input)) return string.Empty;

        return input
            .Replace("&", "&amp;")
            .Replace("<", "&lt;")
            .Replace(">", "&gt;");
    }
}
