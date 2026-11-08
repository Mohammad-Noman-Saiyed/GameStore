using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;

// Id not needed because server usually generates it after creating 
// the resource and gives it back to u
public record CreateGameDto(
    // this "[Required]" (and also using System.CompoennetModel.DataAnnotations up top
    // is done so that ASP.NET Core always makes sure these names are proper
    // so that in we don't already havfe to check if name is empty or not
    // we can now I guess move on with the assumption that it's always non-empty and valid)
    // however, we simply doing this isn't enough and we actually have to activate this
    // and this activation is done by having that builder.Servicer.AddValidation which 
    // checks each endpoint for us I guess
    // this "[StringLength(50)]" checks the name so that it doens't go above 50 characters i think
    [Required][StringLength(50)] string Name,
    [Required][StringLength(20)] string Genre,
    // this means the price has to be between $1 - $100.
    [Range(1, 100)] decimal Price,
    DateOnly ReleaseDate
);