namespace GameLister.Api.Dtos;

public record GameImageDto(
    int Id,
    string Url,
    string Type,
    int SortOrder,
    bool IsPrimary
);

public record GameImageCreateDto(
        string Url,
    string Type,
    int SortOrder,
    bool IsPrimary
);
