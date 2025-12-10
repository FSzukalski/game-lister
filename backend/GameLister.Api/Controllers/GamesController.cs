using GameLister.Api.Data;
using GameLister.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameLister.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GamesController : ControllerBase
{
    private readonly GameListerDbContext _context;

    public GamesController(GameListerDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Game>>> GetAll()
    {
        return await _context.Games
            .OrderByDescending(g => g.CreatedAt)
            .ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Game>> GetById(int id)
    {
        var game = await _context.Games.FindAsync(id);

        if (game == null)
        {
            return NotFound();
        }

        return game;
    }


    [HttpPost]
    public async Task<ActionResult<Game>> Create(Game game)
    {
        game.CreatedAt = DateTime.UtcNow;
        _context.Games.Add(game);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll), new { id = game.Id }, game);
    }
}
