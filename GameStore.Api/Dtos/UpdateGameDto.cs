using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

public record UpdateGameDto(
    [Required][StringLength(50)] string Name,
    [Required][StringLength(20)] string Genre,
    // this means the price has to be between $1 - $100.
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
);