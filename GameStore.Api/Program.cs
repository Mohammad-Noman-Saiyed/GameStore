using Gamestore.Api.Endpoints;
using GameStore.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// this means validation is available  for every single endpoint in our app
builder.Services.AddValidation();
builder.AddGameStoreDb();


var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
