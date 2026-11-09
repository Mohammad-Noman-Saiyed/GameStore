using GameStore.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Data;

public static class DataExntensions
{
    //this migrates database
    public static void MigrateDb(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();
        dbContext.Database.Migrate();
    }

    //this is to regirster database services and initial database seeding
    //                                 vvvvvvvvvvvvvvvv this is being exntended
    //                                                    vvvvvvv building is being extended i tihnk
    public static void AddGameStoreDb(this WebApplicationBuilder builder)
    {
        // has to be before creation of app object
        // not sure what the following 2 lines do
        // btw this was just [var connString = "Data Source=GameStore.db" ]
        // but we changed it so that it's easier to use. We did this by
        // adding a 'connection string' in appsettings.json where a key-value
        // pair was set. the key is  'GameStore' and its value is the "Data Source=GameStore.db"
        // so that we can refer to it. We do so by accessing the Configuration method/field within 'builder'
        // and use the GetConnectionString function where we pass in the key name "GameStore" so it knows what to return 
        // (the value that's associated with GameStore in this case)
        var connString = builder.Configuration.GetConnectionString("GameStore");
        
        //DbContext has a Scoped service lifetime because:
        // 1. It ensures that a new instance of DbContext is created per request
        // 2. DB connections are a limioted and expensive resource
        // 3. DbContext is not thread-safe. Scoped avoids to concurrency issues
        // 4. Makes it easier to manage transaction and ensure data consistency
        // 5. Reusing a DbContext instance can lead to increased memory usage (so a short life-time decreases memory usage)

        // we registered dbContext here
        builder.Services.AddSqlite<GameStoreContext>(
            connString,
            //the below is ond so that when the database is first created,
            // some default genre's are added to the Genre database table
            // this is called Seeding. And this is done so that we don't get error
            // messages and so that can can further add more stuff to the Genre
            // database table i think
            optionsAction: options => options.UseSeeding((context, _) =>
            //                                            ^^^^     ^ this means we're not using it
            //                                            this is one of the parameters we pass
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
            }
}