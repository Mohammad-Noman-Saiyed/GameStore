using Gamestore.Api.Endpoints;
using GameStore.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// this means validation is available  for every single endpoint in our app
builder.Services.AddValidation();

// has to be before creation of app object
// not sure what the following 2 lines do
var connString = "Data Source=GameStore.db";
builder.Services.AddSqlite<GameStoreContext>(connString);

var app = builder.Build();

app.MapGamesEndpoints();

app.Run();
