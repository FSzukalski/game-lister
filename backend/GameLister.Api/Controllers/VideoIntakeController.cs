using GameLister.Api.Data;
using GameLister.Api.Dtos;
using GameLister.Api.Models;
using GameLister.Api.Models.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameLister.Api.Controllers;

[ApiController]
[Route("api/intake")]
public class VideoIntakeController : ControllerBase
{
    private readonly GameListerDbContext _db;

    public VideoIntakeController(GameListerDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// MOCK: przyjmuje wynik analizy wideo (tak jakby od AI)
    /// i tworzy Game + GameImages + ListingDraft.
    /// </summary>
    [HttpPost("mock-from-video")]
    public async Task<ActionResult<VideoIntakeResultDto>> CreateFromVideoAsync(
        [FromBody] VideoAnalysisResultDto dto,
        CancellationToken cancellationToken)
    {
        // 1) Zbuduj Money na podstawie SuggestedPrice (albo default)
        var price = dto.SuggestedPrice is null
            ? new Money { Amount = 0m, Currency = "PLN" }
            : new Money { Amount = dto.SuggestedPrice.Amount, Currency = dto.SuggestedPrice.Currency };

        // 2) Utwórz Game
        var game = new Game
        {
            Title = dto.DetectedTitle ?? dto.RawTitle,
            Subtitle = null,
            Description = dto.SpokenDescription,
            Platform = dto.Platform,
            Edition = dto.Edition,
            Language = dto.Language,
            Region = dto.Region,
            Condition = dto.Condition,
            IsBoxIncluded = dto.IsBoxIncluded,
            IsManualIncluded = dto.IsManualIncluded,
            IsOriginal = dto.IsOriginal,
            Price = price,
            CreatedAt = DateTime.UtcNow
        };

        _db.Games.Add(game);
        await _db.SaveChangesAsync(cancellationToken);

        // 3) Dodaj GameImages z ScreenshotUrls
        if (dto.ScreenshotUrls is not null && dto.ScreenshotUrls.Count > 0)
        {
            var index = 0;
            foreach (var url in dto.ScreenshotUrls)
            {
                var image = new GameImage
                {
                    GameId = game.Id,
                    Url = url,
                    Type = index == 0 ? GameImageType.FrontCover : GameImageType.Other,
                    SortOrder = index,
                    IsPrimary = index == 0,
                    CreatedAt = DateTime.UtcNow
                };

                _db.GameImages.Add(image);
                index++;
            }

            await _db.SaveChangesAsync(cancellationToken);
        }

        // 4) Utwórz ListingDraft dla Allegro
        var listing = new ListingDraft
        {
            GameId = game.Id,
            Marketplace = "allegro",
            Title = game.Title,
            Subtitle = game.Subtitle,
            DescriptionHtml = dto.SpokenDescription, // później możemy generować ładniejszy HTML
            CategoryId = "123456",                   // na razie na sztywno, potem mapowanie kategorii
            Language = "pl-PL",
            Status = "draft",
            Price = price,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _db.ListingDrafts.Add(listing);
        await _db.SaveChangesAsync(cancellationToken);

        var result = new VideoIntakeResultDto(
            GameId: game.Id,
            ListingDraftId: listing.Id
        );

        // Zakładam, że masz GET /api/ListingDrafts/{id}
        return CreatedAtAction(
            actionName: "GetById",
            controllerName: "ListingDrafts",
            routeValues: new { id = listing.Id },
            value: result
        );
    }
}
