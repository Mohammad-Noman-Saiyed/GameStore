namespace Gamestore.Api.Endpoints;


using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameSummaryDto> games = [
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
        group.MapGet("/", async (GameStoreContext dbContext) 
            => await dbContext.Games
                              .Include(game => game.Genre)
                              .Select(game => new GameSummaryDto(
                                game.Id,
                                game.Name,
                                game.Genre!.Name,
                                game.Price,
                                game.ReleaseDate               
                            ))
                            .AsNoTracking()
                            .ToListAsync());



        // GET /games/1    (v           v    must match)
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            // now that we're using dependency injection, by adding the GameStoreContext dbContext as the 2nd parameter,
            // we don't do "var game = games.Find(game => game.Id == id);" but rather:
            
            var game = await dbContext.Games.FindAsync(id);

            return game is null ? Results.NotFound() : Results.Ok(
                new GameDetailsDto(
                    game.Id,
                    game.Name,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                )
            );
        })
            .WithName(GetGameEndpointName); // we can now refer to this route as GetGame

        // POST /games
        group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
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
            // now that we're using dependency injection, we remove all of this (originally this is what we had)
            /*
            GameDto game = new(
                games.Count + 1, // id
                newGame.Name, // name from the newGame
                newGame.Genre, // genre from the newGame
                newGame.Price, // price from the newGame
                newGame.ReleaseDate // release date from the newGame
            );
            */

            Game game = new()
            {
                // we don't create an id or set one because the database creates one as soon as the game created over there
                Name = newGame.Name,
                GenreId = newGame.GenreId,
                Price = newGame.Price,
                ReleaseDate = newGame.ReleaseDate
            };


            // instead of this "games.Add(game);" we do the following:
            dbContext.Games.Add(game);
            // the above doesn't actually send the game into the database
            // it only asks entity, framework core to start keeping track of the fact that
            // a new agme needs to be inserted into the database. To actually make the call
            // to the DB so that the game is saved is:
            await dbContext.SaveChangesAsync();

            // after the dbContext.SaveChange() above, the id is auto-generated and i guess availble to use
            GameDetailsDto gameDto = new(
                game.Id,
                game.Name,
                game.GenreId,
                game.Price,
                game.ReleaseDate
            );

            //use the route, send the Id, and then the payload
            // this is the 201 JSON code thingy
            // instead of returning "game" in "Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);" we make
            // a new Dto (GameDetailsDto in our case) and return that instead so that the user cannot see internal details I tihnk
            return Results.CreatedAtRoute(GetGameEndpointName, new {id = gameDto.Id}, gameDto);
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
            
            games[index] = new GameSummaryDto(
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