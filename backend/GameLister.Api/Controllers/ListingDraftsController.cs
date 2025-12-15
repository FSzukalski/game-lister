using GameLister.Api.Data;
using GameLister.Api.Dtos;
using GameLister.Api.Models;
using GameLister.Api.Models.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameLister.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingDraftsController : ControllerBase
{
    private readonly GameListerDbContext _context;

    public ListingDraftsController(GameListerDbContext context)
    {
        _context = context;
    }

    // GET: /api/ListingDrafts?gameId=1
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListingDraftDto>>> GetAll([FromQuery] int? gameId)
    {
        var query = _context.ListingDrafts.AsQueryable();

        if (gameId.HasValue)
        {
            query = query.Where(ld => ld.GameId == gameId.Value);
        }

        var drafts = await query
            .OrderByDescending(ld => ld.CreatedAt)
            .ToListAsync();

        return drafts.Select(ToDto).ToList();
    }

    // GET: /api/ListingDrafts/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ListingDraftDto>> GetById(int id)
    {
        var d = await _context.ListingDrafts.FindAsync(id);
        if (d is null) return NotFound();

        return ToDto(d);
    }

    // POST: /api/ListingDrafts
    [HttpPost]
    public async Task<ActionResult<ListingDraftDto>> Create(ListingDraftUpsertDto request)
    {
        // Sprawdzamy, czy istnieje powiązana gra
        var game = await _context.Games.FindAsync(request.GameId);
        if (game is null)
        {
            return BadRequest($"Game with id {request.GameId} not found.");
        }

        var now = DateTime.UtcNow;

        var draft = new ListingDraft
        {
            GameId = request.GameId,
            Marketplace = request.Marketplace,
            Title = request.Title,
            Subtitle = request.Subtitle,
            DescriptionHtml = request.DescriptionHtml,
            CategoryId = request.CategoryId,
            Language = request.Language,
            Status = request.Status,
            Price = new Money
            {
                Amount = request.Price.Amount,
                Currency = request.Price.Currency
            },
            CreatedAt = now,
            UpdatedAt = now
        };

        _context.ListingDrafts.Add(draft);
        await _context.SaveChangesAsync();

        var dto = ToDto(draft);

        return CreatedAtAction(
            nameof(GetById),
            new { id = draft.Id },
            dto
        );
    }

    // PUT: /api/ListingDrafts/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ListingDraftDto>> Update(int id, ListingDraftUpsertDto request)
    {
        var draft = await _context.ListingDrafts.FindAsync(id);
        if (draft is null) return NotFound();

        // (opcjonalnie) możesz wymusić, że GameId nie zmieniamy
        if (draft.GameId != request.GameId)
        {
            // Możesz wybrać: albo blokujesz zmianę, albo ją dopuszczasz.
            // Ja dla prostoty dopuszczam zmianę:
            var game = await _context.Games.FindAsync(request.GameId);
            if (game is null)
            {
                return BadRequest($"Game with id {request.GameId} not found.");
            }

            draft.GameId = request.GameId;
        }

        draft.Marketplace = request.Marketplace;
        draft.Title = request.Title;
        draft.Subtitle = request.Subtitle;
        draft.DescriptionHtml = request.DescriptionHtml;
        draft.CategoryId = request.CategoryId;
        draft.Language = request.Language;
        draft.Status = request.Status;
        draft.Price = new Money
        {
            Amount = request.Price.Amount,
            Currency = request.Price.Currency
        };
        draft.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return ToDto(draft);
    }

    // DELETE: /api/ListingDrafts/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var draft = await _context.ListingDrafts.FindAsync(id);
        if (draft is null) return NotFound();

        _context.ListingDrafts.Remove(draft);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    // ------- mapowanie Entity -> DTO -------

    private static ListingDraftDto ToDto(ListingDraft d)
        => new(
            d.Id,
            d.GameId,
            d.Marketplace,
            d.Title,
            d.Subtitle,
            d.DescriptionHtml,
            d.CategoryId,
            d.Language,
            d.Status,
            new MoneyDto(d.Price.Amount, d.Price.Currency),
            d.CreatedAt,
            d.UpdatedAt
        );
}
