using GameLister.Api.Models.ValueObjects;

namespace GameLister.Api.Models;

public class ListingDraft
{
    public int Id { get; set; }

    // Powiązanie z Game – to jest „źródło prawdy” o egzemplarzu gry
    public int GameId { get; set; }
    public Game Game { get; set; } = null!;

    /// <summary>
    /// Marketplace docelowy, np. "Allegro", "OLX".
    /// Na razie będzie to głównie "Allegro".
    /// </summary>
    public string Marketplace { get; set; } = "Allegro";

    // ---- Dane aukcji (pod Allegro/OLX) ----

    /// <summary>
    /// Tytuł aukcji (to, co zobaczy kupujący w listingu).
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Opcjonalny podtytuł (jeżeli marketplace obsługuje).
    /// </summary>
    public string? Subtitle { get; set; }

    /// <summary>
    /// Opis w HTML/Markdown – z tego będziemy później budować strukturę
    /// opisu wymaganą przez Allegro.
    /// </summary>
    public string DescriptionHtml { get; set; } = string.Empty;

    /// <summary>
    /// Id kategorii marketplace (dla Allegro: category.id).
    /// </summary>
    public string CategoryId { get; set; } = string.Empty;

    /// <summary>
    /// Język aukcji w formacie BCP-47, np. "pl-PL".
    /// </summary>
    public string Language { get; set; } = "pl-PL";

    /// <summary>
    /// Cena na aukcji – może być równa Game.Price albo inna (promocja, pakiet).
    /// </summary>
    public Money Price { get; set; } = new();

    /// <summary>
    /// Ilość sztuk dostępnych na aukcji.
    /// </summary>
    public int Quantity { get; set; } = 1;

    /// <summary>
    /// Identyfikator cennika wysyłki (shippingRates.id na Allegro),
    /// który kiedyś pobierzemy z ich API.
    /// Na razie będzie wpisywany ręcznie.
    /// </summary>
    public string? ShippingRateId { get; set; }

    /// <summary>
    /// Status szkicu – na razie tekstowo, później możemy zrobić enum.
    /// "Draft", "ReadyToPublish", "Published", "Archived", itp.
    /// </summary>
    public string Status { get; set; } = "Draft";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
