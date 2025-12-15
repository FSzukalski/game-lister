using GameLister.Api.Models.ValueObjects;

namespace GameLister.Api.Models.Allegro;

public class AllegroOfferData
{
    // Podstawowe
    public string Title { get; set; } = default!;
    public string DescriptionHtml { get; set; } = default!;
    public string CategoryId { get; set; } = default!;
    public string Language { get; set; } = "pl-PL";

    // Cena – używamy naszego ValueObject
    public Money Price { get; set; } = new();

    // Stan / kompletność – uproszczone na razie
    public string Condition { get; set; } = "USED";
    public bool IsBoxIncluded { get; set; }
    public bool IsManualIncluded { get; set; }
    public bool IsOriginal { get; set; }

    // Obrazy – na razie tylko URL-e, później połączymy z video/AI
    public List<string> ImageUrls { get; set; } = [];

    // Lokalizacja sprzedawcy – na razie stała / konfigurowalna
    public string LocationCountryCode { get; set; } = "PL";
    public string LocationProvince { get; set; } = "pomorskie";
    public string LocationCity { get; set; } = "Gdańsk";

    // Prosty model dostawy – pole do dalszej rozbudowy
    public string? ShippingPolicyId { get; set; }
}
