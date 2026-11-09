using Gamestore.Api.Endpoints;
using GameStore.Api.Data;
using GameStore.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

// this means validation is available  for every single endpoint in our app
builder.Services.AddValidation();
builder.AddGameStoreDb();


var app = builder.Build();

app.MapGamesEndpoints();
app.MapeGenresEndpoints();

app.MigrateDb();

app.Run();
