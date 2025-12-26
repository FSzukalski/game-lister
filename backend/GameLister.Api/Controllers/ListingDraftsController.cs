using GameLister.Api.Data;
using GameLister.Api.Dtos;
using GameLister.Api.Models;
using GameLister.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace GameLister.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ListingDraftsController : ControllerBase
{
    private readonly GameListerDbContext _db;
    private readonly IListingPreviewService _listingPreviewService;
    private readonly IAllegroOfferPayloadService _allegroOfferPayloadService;

    public ListingDraftsController(
        GameListerDbContext db,
        IListingPreviewService listingPreviewService,
        IAllegroOfferPayloadService allegroOfferPayloadService)
    {
        _db = db;
        _listingPreviewService = listingPreviewService;
        _allegroOfferPayloadService = allegroOfferPayloadService;
    }

    // GET: api/ListingDrafts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ListingDraftDto>>> GetAll(CancellationToken cancellationToken)
    {
        var drafts = await _db.ListingDrafts
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var dtos = drafts.Select(d => new ListingDraftDto(
            Id: d.Id,
            GameId: d.GameId,
            Marketplace: d.Marketplace,
            Title: d.Title,
            Subtitle: d.Subtitle,
            DescriptionHtml: d.DescriptionHtml,
            CategoryId: d.CategoryId,
            Language: d.Language,
            Status: d.Status,
            Price: new MoneyDto(d.Price.Amount, d.Price.Currency),
            CreatedAt: d.CreatedAt,
            UpdatedAt: d.UpdatedAt
        ));

        return Ok(dtos);
    }

    // GET: api/ListingDrafts/5
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ListingDraftDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var d = await _db.ListingDrafts.FindAsync(new object[] { id }, cancellationToken);
        if (d is null)
            return NotFound();

        var dto = new ListingDraftDto(
            Id: d.Id,
            GameId: d.GameId,
            Marketplace: d.Marketplace,
            Title: d.Title,
            Subtitle: d.Subtitle,
            DescriptionHtml: d.DescriptionHtml,
            CategoryId: d.CategoryId,
            Language: d.Language,
            Status: d.Status,
            Price: new MoneyDto(d.Price.Amount, d.Price.Currency),
            CreatedAt: d.CreatedAt,
            UpdatedAt: d.UpdatedAt
        );

        return Ok(dto);
    }

    // POST: api/ListingDrafts
    [HttpPost]
    public async Task<ActionResult<ListingDraftDto>> Create(
        ListingDraftUpsertDto dto,
        CancellationToken cancellationToken)
    {
        var draft = new ListingDraft
        {
            GameId = dto.GameId,
            Marketplace = dto.Marketplace,
            Title = dto.Title,
            Subtitle = dto.Subtitle,
            DescriptionHtml = dto.DescriptionHtml,
            CategoryId = dto.CategoryId,
            Language = dto.Language,
            Status = dto.Status,
            Price = new GameLister.Api.Models.ValueObjects.Money
            {
                Amount = dto.Price.Amount,
                Currency = dto.Price.Currency
            },
            CreatedAt = DateTime.UtcNow
        };

        _db.ListingDrafts.Add(draft);
        await _db.SaveChangesAsync(cancellationToken);

        var result = new ListingDraftDto(
            Id: draft.Id,
            GameId: draft.GameId,
            Marketplace: draft.Marketplace,
            Title: draft.Title,
            Subtitle: draft.Subtitle,
            DescriptionHtml: draft.DescriptionHtml,
            CategoryId: draft.CategoryId,
            Language: draft.Language,
            Status: draft.Status,
            Price: new MoneyDto(draft.Price.Amount, draft.Price.Currency),
            CreatedAt: draft.CreatedAt,
            UpdatedAt: draft.UpdatedAt
        );

        return CreatedAtAction(nameof(GetById), new { id = draft.Id }, result);
    }

    // PUT: api/ListingDrafts/5
    [HttpPut("{id:int}")]
    public async Task<ActionResult<ListingDraftDto>> Update(
        int id,
        ListingDraftUpsertDto dto,
        CancellationToken cancellationToken)
    {
        var draft = await _db.ListingDrafts.FindAsync(new object[] { id }, cancellationToken);
        if (draft is null)
            return NotFound();

        draft.GameId = dto.GameId;
        draft.Marketplace = dto.Marketplace;
        draft.Title = dto.Title;
        draft.Subtitle = dto.Subtitle;
        draft.DescriptionHtml = dto.DescriptionHtml;
        draft.CategoryId = dto.CategoryId;
        draft.Language = dto.Language;
        draft.Status = dto.Status;
        draft.Price.Amount = dto.Price.Amount;
        draft.Price.Currency = dto.Price.Currency;
        draft.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync(cancellationToken);

        var result = new ListingDraftDto(
            Id: draft.Id,
            GameId: draft.GameId,
            Marketplace: draft.Marketplace,
            Title: draft.Title,
            Subtitle: draft.Subtitle,
            DescriptionHtml: draft.DescriptionHtml,
            CategoryId: draft.CategoryId,
            Language: draft.Language,
            Status: draft.Status,
            Price: new MoneyDto(draft.Price.Amount, draft.Price.Currency),
            CreatedAt: draft.CreatedAt,
            UpdatedAt: draft.UpdatedAt
        );

        return Ok(result);
    }

    // DELETE: api/ListingDrafts/5
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var draft = await _db.ListingDrafts.FindAsync(new object[] { id }, cancellationToken);
        if (draft is null)
            return NotFound();

        _db.ListingDrafts.Remove(draft);
        await _db.SaveChangesAsync(cancellationToken);

        return NoContent();
    }

    // GET: api/ListingDrafts/5/preview
    [HttpGet("{id:int}/preview")]
    public async Task<ActionResult<ListingPreviewDto>> GetPreview(
        int id,
        CancellationToken cancellationToken)
    {
        var preview = await _listingPreviewService.GetPreviewAsync(id, cancellationToken);
        if (preview is null)
            return NotFound();

        return Ok(preview);
    }

    // GET: api/ListingDrafts/5/allegro-payload
    [HttpGet("{id:int}/allegro-payload")]
    public async Task<ActionResult<AllegroOfferPayloadDto>> GetAllegroPayload(
    int id,
    CancellationToken cancellationToken)
    {
        var payload = await _allegroOfferPayloadService.BuildAllegroOfferPayloadAsync(id, cancellationToken);

        if (payload is null)
        {
            return NotFound();
        }

        return Ok(payload);
    }

    

}
