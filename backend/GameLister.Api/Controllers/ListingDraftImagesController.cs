using GameLister.Api.Data;
using GameLister.Api.Dtos;
using GameLister.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameLister.Api.Controllers;

[ApiController]
[Route("api/ListingDrafts/{id:int}/images")]
public class ListingDraftImagesController : ControllerBase
{
    private readonly GameListerDbContext _db;

    public ListingDraftImagesController(GameListerDbContext db)
    {
        _db = db;
    }

    // GET /api/ListingDrafts/{id}/images
    // Zwraca obrazy wybrane dla draftu; jeśli draft nie ma przypiętych obrazów -> fallback: Game.Images
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListingDraftImageDto>>> Get(int id, CancellationToken ct)
    {
        var draft = await _db.ListingDrafts
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (draft is null)
            return NotFound();

        // 1) Spróbuj wziąć obrazy przypięte do draftu
        var selected = await _db.ListingDraftImages
            .AsNoTracking()
            .Where(x => x.ListingDraftId == id)
            .Include(x => x.GameImage)
            .OrderBy(x => x.SortOrder)
            .Select(x => new ListingDraftImageDto(
                x.GameImageId,
                x.GameImage.Url,
                x.GameImage.Type.ToString(),
                x.SortOrder,
                x.GameImage.IsPrimary
            ))
            .ToListAsync(ct);

        if (selected.Count > 0)
            return Ok(selected);

        // 2) Fallback: jeśli nic nie wybrane, zwróć obrazy z gry
        var fallback = await _db.GameImages
            .AsNoTracking()
            .Where(i => i.GameId == draft.GameId)
            .OrderByDescending(i => i.IsPrimary)
            .ThenBy(i => i.SortOrder)
            .ThenBy(i => i.Id)
            .Select(i => new ListingDraftImageDto(
                i.Id,
                i.Url,
                i.Type.ToString(),
                i.SortOrder,
                i.IsPrimary
            ))
            .ToListAsync(ct);

        return Ok(fallback);
    }

    // PUT /api/ListingDrafts/{id}/images
    // Ustawia listę obrazów dla draftu (selekcja + kolejność)
    [HttpPut]
    public async Task<ActionResult<IEnumerable<ListingDraftImageDto>>> Put(
        int id,
        [FromBody] ListingDraftImagesUpsertDto dto,
        CancellationToken ct)
    {
        var draft = await _db.ListingDrafts
            .FirstOrDefaultAsync(x => x.Id == id, ct);

        if (draft is null)
            return NotFound();

        var items = dto?.Items ?? new List<ListingDraftImageUpsertItemDto>();

        // Duplikaty GameImageId
        var duplicates = items
            .GroupBy(x => x.GameImageId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicates.Count > 0)
            return BadRequest(new { message = "Duplicate GameImageId in request.", duplicates });

        if (items.Any(x => x.SortOrder < 0))
            return BadRequest(new { message = "SortOrder must be >= 0." });

        var imageIds = items.Select(x => x.GameImageId).ToList();

        // Sprawdź czy wszystkie obrazki istnieją
        var images = await _db.GameImages
            .Where(i => imageIds.Contains(i.Id))
            .ToListAsync(ct);

        if (images.Count != imageIds.Count)
        {
            var missing = imageIds.Except(images.Select(i => i.Id)).ToList();
            return BadRequest(new { message = "Some GameImageId do not exist.", missing });
        }

        // Sprawdź czy wszystkie obrazki należą do tej samej gry co draft
        var wrongGame = images
            .Where(i => i.GameId != draft.GameId)
            .Select(i => i.Id)
            .ToList();

        if (wrongGame.Count > 0)
            return BadRequest(new { message = "Some images do not belong to the draft's GameId.", wrongGame });

        await using var tx = await _db.Database.BeginTransactionAsync(ct);

        // Wyczyść poprzednie linki
        var existing = await _db.ListingDraftImages
            .Where(x => x.ListingDraftId == id)
            .ToListAsync(ct);

        _db.ListingDraftImages.RemoveRange(existing);

        // Dodaj nowe
        var links = items
            .OrderBy(x => x.SortOrder)
            .Select(x => new ListingDraftImage
            {
                ListingDraftId = id,
                GameImageId = x.GameImageId,
                SortOrder = x.SortOrder,
                CreatedAt = DateTime.UtcNow
            })
            .ToList();

        _db.ListingDraftImages.AddRange(links);

        draft.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        // Zwróć aktualny stan
        var result = await _db.ListingDraftImages
            .AsNoTracking()
            .Where(x => x.ListingDraftId == id)
            .Include(x => x.GameImage)
            .OrderBy(x => x.SortOrder)
            .Select(x => new ListingDraftImageDto(
                x.GameImageId,
                x.GameImage.Url,
                x.GameImage.Type.ToString(),
                x.SortOrder,
                x.GameImage.IsPrimary
            ))
            .ToListAsync(ct);

        return Ok(result);
    }
}
