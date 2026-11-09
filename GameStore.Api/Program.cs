using Gamestore.Api.Endpoints;
using GameStore.Api.Data;
using GameStore.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// this means validation is available  for every single endpoint in our app
builder.Services.AddValidation();

// has to be before creation of app object
// not sure what the following 2 lines do
var connString = "Data Source=GameStore.db";
builder.Services.AddSqlite<GameStoreContext>(
    connString,
    //the below is ond so that when the database is first created,
    // some default genre's are added to the Genre database table
    // this is called Seeding. And this is done so that we don't get error
    // messages and so that can can further add more stuff to the Genre
    // database table i think
    optionsAction: options => options.UseSeeding((context, _) =>
    {
        if (!context.Set<Genre>().Any())
        {
            context.Set<Genre>().AddRange(
                new Genre { Name = "Fighting" },
                new Genre { Name = "RPG" },
                new Genre { Name= "Platformer" },
                new Genre { Name = "Racing" },
                new Genre { Name = "Sports" }
            );
            
            //after manually adding the above Genre's to the Genre database table,
            // u gotta save the changes to actually apply them I think
            context.SaveChanges();
        }
    })
);

var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
