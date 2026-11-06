namespace GameStore.Api.Dtos;

public record UpdateGameDto(
    string Name,
    String Genre,
    decimal Price,
    DateOnly ReleaseDate
);