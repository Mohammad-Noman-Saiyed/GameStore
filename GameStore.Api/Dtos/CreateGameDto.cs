namespace GameStore.Api.Dtos;

// Id not needed because server usually generates it after creating 
// the resource and gives it back to u
public record CreateGameDto(
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
);