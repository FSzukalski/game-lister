using System.Collections.Generic;

namespace GameLister.Api.Dtos;

public record VideoAnalysisResultDto(
    string RawTitle,                 // np. "Mario Kart 8 na Switcha, stan bdb"
    string? DetectedTitle,           // np. "Mario Kart 8 Deluxe"
    string Platform,                 // np. "Nintendo Switch"
    string? Edition,                 // np. "Deluxe"
    string? Language,                // np. "PL/EN"
    string? Region,                  // np. "PAL"
    string Condition,                // np. "Very good"
    bool IsBoxIncluded,
    bool IsManualIncluded,
    bool IsOriginal,
    MoneyDto? SuggestedPrice,        // jeśli AI poda sugerowaną cenę
    string? SpokenDescription,       // opis z transkrypcji mowy
    IReadOnlyList<string> ScreenshotUrls // URL-e obrazków wyciągniętych z video
);

public record VideoIntakeResultDto(
    int GameId,
    int ListingDraftId
// później możesz tu dorzucić np. PreviewUrl, AllegroPayloadUrl itd.
);
