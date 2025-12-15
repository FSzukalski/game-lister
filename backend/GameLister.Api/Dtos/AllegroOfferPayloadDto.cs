using GameLister.Api.Models.ValueObjects;

namespace GameLister.Api.Dtos;

public record AllegroOfferPayloadDto(
    string Title,
    string DescriptionHtml,
    string CategoryId,
    string Language,
    MoneyDto Price,
    string Condition,
    bool IsBoxIncluded,
    bool IsManualIncluded,
    bool IsOriginal,
    IReadOnlyList<string> ImageUrls,
    string LocationCountryCode,
    string LocationProvince,
    string LocationCity,
    string? ShippingPolicyId
);
