using GameLister.Api.Models.ValueObjects;

namespace GameLister.Api.Models.Allegro;

// To jest “esencja” payloadu, który wyślesz do /sale/product-offers
public class AllegroOfferData
{
    // Część produktowa
    public AllegroProductData Product { get; set; } = new();

    // Część ofertowa
    public AllegroSellingMode SellingMode { get; set; } = new();
    public List<string> Images { get; set; } = new();          // URL-e zdjęć
    public AllegroContact Contact { get; set; } = new();
    public AllegroLocation? Location { get; set; }
}

public class AllegroProductData
{
    public string? ProductId { get; set; }                     // jeśli użyjesz katalogu Allegro
    public string? Name { get; set; }                          // tytuł
    public string? CategoryId { get; set; }                    // np. id kategorii gier
    public List<AllegroParameter> Parameters { get; set; } = new();
}

public class AllegroSellingMode
{
    public string Format { get; set; } = "ADVERTISEMENT";      // zgodnie z dokumentacją
    public Money Price { get; set; } = new();
}

public class AllegroContact
{
    public string Name { get; set; } = string.Empty;           // np. nazwa profilu sprzedawcy
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}

public class AllegroLocation
{
    public string? City { get; set; }
    public string? PostCode { get; set; }
    public string? CountryCode { get; set; } = "PL";
}

public class AllegroParameter
{
    public string Id { get; set; } = string.Empty;             // np. "11323" dla parametru "Stan"
    public List<string>? Values { get; set; }                  // wartości tekstowe
    public List<string>? ValuesIds { get; set; }               // słownikowe id, np. "11323_1" -> "Nowy"
}
