using GameLister.Api.Data;
using GameLister.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameLister.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly GameListerDbContext _db;

    public GamesController(GameListerDbContext db)
    {
        _db = db;
    }

    // GET: /api/Games
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Game>>> GetAll()
    {
        var games = await _db.Games
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();

        return Ok(games);
    }

    // GET: /api/Games/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Game>> GetById(int id)
    {
        var game = await _db.Games.FindAsync(id);
        if (game == null) return NotFound();
        return Ok(game);
    }

    // POST: /api/Games
    [HttpPost]
    public async Task<ActionResult<Game>> Create(Game game)
    {
        // CreatedAt ustawiamy po stronie backendu
        game.CreatedAt = DateTime.UtcNow;

        _db.Games.Add(game);
        await _db.SaveChangesAsync();

        // Zwracamy 201 + Location header do GET /api/Games/{id}
        return CreatedAtAction(nameof(GetById), new { id = game.Id }, game);
    }

    // PUT: /api/Games/1
    [HttpPut("{id:int}")]
    public async Task<ActionResult<Game>> Update(int id, Game updated)
    {
        var game = await _db.Games.FindAsync(id);
        if (game == null) return NotFound();

        game.Title = updated.Title;
        game.Description = updated.Description;
        game.Platform = updated.Platform;
        game.Condition = updated.Condition;
        game.Price = updated.Price;

        await _db.SaveChangesAsync();

        return Ok(game);
    }

    // DELETE: /api/Games/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var game = await _db.Games.FindAsync(id);
        if (game == null) return NotFound();

        _db.Games.Remove(game);
        await _db.SaveChangesAsync();

        return NoContent(); // 204
    }
}
