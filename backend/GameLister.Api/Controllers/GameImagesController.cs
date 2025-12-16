using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GameLister.Api.Data;
using GameLister.Api.Dtos;
using GameLister.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameLister.Api.Controllers;

[ApiController]
[Route("api/games/{gameId:int}/images")]
public class GameImagesController : ControllerBase
{
    private readonly GameListerDbContext _db;

    public GameImagesController(GameListerDbContext db)
    {
        _db = db;
    }

    // GET /api/games/{gameId}/images
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameImageDto>>> GetImages(
        int gameId,
        CancellationToken cancellationToken)
    {
        var gameExists = await _db.Games
            .AsNoTracking()
            .AnyAsync(g => g.Id == gameId, cancellationToken);

        if (!gameExists)
        {
            return NotFound();
        }

        var images = await _db.GameImages
            .AsNoTracking()
            .Where(i => i.GameId == gameId)
            .OrderByDescending(i => i.IsPrimary)
            .ThenBy(i => i.SortOrder)
            .ThenBy(i => i.Id)
            .ToListAsync(cancellationToken);

        var dtos = images.Select(i => new GameImageDto(
            i.Id,
                        i.Url,
            i.Type.ToString(),
            i.SortOrder,
            i.IsPrimary));

        return Ok(dtos);
    }

    // POST /api/games/{gameId}/images
    [HttpPost]
    public async Task<ActionResult<GameImageDto>> AddImage(
        int gameId,
        [FromBody] GameImageCreateDto dto,
        CancellationToken cancellationToken)
    {


        var game = await _db.Games.FindAsync(new object[] { gameId }, cancellationToken);
        if (game is null)
        {
            return NotFound();
        }

        var image = new GameImage
        {
            GameId = gameId,
            Url = dto.Url,
            Type = MapType(dto.Type),   // << kluczowa linia
            SortOrder = dto.SortOrder,
            IsPrimary = dto.IsPrimary,
            CreatedAt = DateTime.UtcNow
        };


        // Jeśli ustawiamy nowe zdjęcie jako główne – zdejmujemy flagę z poprzednich
        if (image.IsPrimary)
        {
            var existingPrimary = await _db.GameImages
                .Where(i => i.GameId == gameId && i.IsPrimary)
                .ToListAsync(cancellationToken);

            foreach (var img in existingPrimary)
            {
                img.IsPrimary = false;
            }
        }

        _db.GameImages.Add(image);
        await _db.SaveChangesAsync(cancellationToken);

        var resultDto = new GameImageDto(
            image.Id,

                        image.Url,
            image.Type.ToString(),
            image.SortOrder,
            image.IsPrimary);

        return CreatedAtAction(nameof(GetImages), new { gameId }, resultDto);
    }

    private static GameImageType MapType(string type)
    {
        if (string.IsNullOrWhiteSpace(type))
            return GameImageType.Unknown;

        return type.Trim().ToLowerInvariant() switch
        {
            // front
            "front" or "frontcover" or "front_cover" or "cover"
                => GameImageType.FrontCover,

            // back
            "back" or "backcover" or "back_cover"
                => GameImageType.BackCover,

            // disc / cartridge
            "disc" or "cartridge" or "media"
                => GameImageType.DiscOrCartridge,

            // manual
            "manual" or "instruction" or "instrukcja"
                => GameImageType.Manual,

            // box / boxset
            "box" or "boxset" or "bigbox"
                => GameImageType.BoxSet,

            // screenshot
            "screenshot" or "ingame" or "in_game"
                => GameImageType.InGameScreenshot,

            // fallback
            _ => GameImageType.Other
        };
    }


}


