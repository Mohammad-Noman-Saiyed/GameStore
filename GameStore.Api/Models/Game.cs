namespace GameStore.Api.Models;

public class Game
{
    public int Id { get; set; }

    // string? means nullable value means the string can be null
    // the required means the name is required
    public required string Name { get; set; } 

    // this vvv is the genre type from Genre.cs I think
    public Genre? Genre { get; set; }

    // soetimes u may only want the genre id and not the genre name
    // so i guess we can just gave the id so that we dont have to render
    // the genre every time
    public int GenreId { get; set; }

    public decimal Price { get; set; }

    public DateOnly ReleaseDate { get; set; }

}