using GameLister.Api.Models.ValueObjects;

namespace GameLister.Api.Models;

public class Game
{
    public int Id { get; set; }

    // Podstawowe dane o grze
    public string Title { get; set; } = string.Empty;         // np. "Mario Kart 8 Deluxe"
    public string? Subtitle { get; set; }                     // np. "PL, komplet, stan bdb"
    public string? Description { get; set; }                  // pełny opis aukcyjny

    // Dane “techniczne”
    public string? Platform { get; set; }                     // Switch / PS5 / Xbox / PC
    public string? Edition { get; set; }                      // np. "Limited", "Collector's Edition"
    public string? Language { get; set; }                     // np. "PL", "EN", "PL/ENG"
    public string? Region { get; set; }                       // np. "PAL", "NTSC-U"

    // Stan fizyczny
    public string? Condition { get; set; }                    // np. "Bardzo dobry", "Nowy"
    public bool IsBoxIncluded { get; set; }                   // czy jest pudełko
    public bool IsManualIncluded { get; set; }                // czy jest instrukcja
    public bool IsOriginal { get; set; } = true;              // oryginalna gra, nie kopia

    // Cena
    public Money Price { get; set; } = new();                 // np. 129.99 PLN

    // Dane pomocnicze
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Relacja do zdjęć
    public ICollection<GameImage> Images { get; set; } = new List<GameImage>();

    // Relacja do stworzonych aukcji

    public ICollection<ListingDraft> ListingDrafts { get; set; } = new List<ListingDraft>();
    }
