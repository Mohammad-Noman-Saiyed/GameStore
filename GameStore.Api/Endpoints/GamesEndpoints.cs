namespace Gamestore.Api.Endpoints;

using GameStore.Api.Dtos;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
        new (
            1, 
            "Street Fighter II", 
            "Fighting", 
            19.99M, // this M is done to show that we wanna use a decimal and not a double
            new DateOnly(1992, 7, 15)),  
        new (
            2, "Final Fantasy VII Rebirth",
            "RPG",
            69.99M,
            new DateOnly(2024, 2, 29)),
        new (
            3, 
            "Astro Bot",
            "platformer",
            59.99M,
            new DateOnly(2024, 9, 6))
    ];

    public static void MapGamesEndpoints(this WebApplication app)
    {

        // instead of doing app.MapGet/Put...("/games", ...)
        // we can just group it so that we can instead do group.MapGet(...)
        // but this time we don't have to write the "/games" in the brackets as
        // it's inherited. So you still can technically do app.MapGet("/games", () => games)
        // but that's not necessary and not ideal in the real world
        var group = app.MapGroup("/games");
        


        // GET /games        vvvvvvvvvvvvvvvvvvvvv handler
        group.MapGet("/", () => games);



        // GET /games/1    (v           v    must match)
        group.MapGet("/{id}", (int id) =>
        {
            var game = games.Find(game => game.Id == id);

            return game is null ? Results.NotFound() : Results.Ok(game);
        })
            .WithName(GetGameEndpointName); // we can now refer to this route as GetGame

        // POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {

            // if the new name for example is empty, we don't wanna go ahead with that update
            // so we can have this classic if statement to check the name. However, we would then
            // want this for every other field such as price, id, genre, etc.
            // but this would require MANY if statements all around the file so we can do something else
            /*
            if (string.IsNullOrEmpty(newGame.Name))
            {
                return Results.BadRequest("Name is required.");
            }
            */

            // to insert a new game, just add a new game to the end of the existing games list
            GameDto game = new(
                games.Count + 1, // id
                newGame.Name, // name from the newGame
                newGame.Genre, // genre from the newGame
                newGame.Price, // price from the newGame
                newGame.ReleaseDate // release date from the newGame
            );

            games.Add(game);

            //use the route, send the Id, and then the payload
            // this is the 201 JSON code thingy
            return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);
        });

        // PUT /games/1
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            // find the index of       GAME where GAME's id is equal to the id passed
            var index = games.FindIndex(game => game.Id == id);
            
            if(index == -1)
            {
                return Results.NotFound();
            }
            
            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );

            // return the 204 JSON code thingy
            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });
    }

}