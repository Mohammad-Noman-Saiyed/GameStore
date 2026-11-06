using GameStore.Api.Dtos;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

List<GameDto> games = [
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

// GET /games        vvvvvvvvvvvvvvvvvvvvv handler
app.MapGet("/games", () => games);

const string GetGameEndpointName = "GetGame";

// GET /games/1    (v           v    must match)
app.MapGet("/games/{id}", (int id) => games.Find(game => game.Id == id))
    .WithName(GetGameEndpointName); // we can now refer to this route as GetGame

// POST /games
app.MapPost("/games", (CreateGameDto newGame) =>
{
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
app.MapPut("/games/{id}", (int id, UpdateGameDto updatedGame) =>
{
    // find the index of       GAME where GAME's id is equal to the id passed
    var index = games.FindIndex(game => game.Id == id);
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

app.Run();
