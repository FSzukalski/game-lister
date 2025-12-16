using System.Collections.Generic;

namespace GameLister.Api.Dtos;

public record AllegroOfferPayloadDto(
    string Title,
    string DescriptionHtml,
    string CategoryId,
    string Language,
    MoneyDto Price,
    IReadOnlyList<string> ImageUrls
);
