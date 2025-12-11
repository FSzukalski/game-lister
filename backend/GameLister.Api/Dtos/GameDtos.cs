using System.ComponentModel.DataAnnotations;

namespace GameLister.Api.Dtos;

public class GameDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public string? Platform { get; set; }
    public string? Condition { get; set; }
    public double? Price { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateGameDto
{
    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string Title { get; set; } = default!;

    [StringLength(2000)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Platform { get; set; }

    [StringLength(100)]
    public string? Condition { get; set; }

    [Range(0, 10000)]
    public double? Price { get; set; }
}

public class UpdateGameDto
{
    [Required]
    [StringLength(200, MinimumLength = 2)]
    public string Title { get; set; } = default!;

    [StringLength(2000)]
    public string? Description { get; set; }

    [StringLength(100)]
    public string? Platform { get; set; }

    [StringLength(100)]
    public string? Condition { get; set; }

    [Range(0, 10000)]
    public double? Price { get; set; }
}
