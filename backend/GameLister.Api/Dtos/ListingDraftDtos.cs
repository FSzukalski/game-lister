using GameLister.Api.Models.ValueObjects;

namespace GameLister.Api.Dtos;

public record MoneyDto(decimal Amount, string Currency);

public record ListingDraftDto(
    int Id,
    int GameId,
    string Marketplace,
    string Title,
    string? Subtitle,
    string? DescriptionHtml,
    string CategoryId,
    string Language,
    string Status,
    MoneyDto Price,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

// Użyjemy tego samego DTO do POST i PUT – tzw. Upsert
public record ListingDraftUpsertDto(
    int GameId,
    string Marketplace,
    string Title,
    string? Subtitle,
    string? DescriptionHtml,
    string CategoryId,
    string Language,
    string Status,
    MoneyDto Price
);
