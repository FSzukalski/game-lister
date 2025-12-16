using System;
using System.Collections.Generic;

namespace GameLister.Api.Dtos;

public record ListingPreviewDto(
    int ListingDraftId,
    int GameId,
    string Marketplace,
    string Title,
    string? Subtitle,
    string DescriptionHtml,
    MoneyDto Price,
    DateTime GeneratedAtUtc,
    IReadOnlyList<GameImageDto> Images
);
