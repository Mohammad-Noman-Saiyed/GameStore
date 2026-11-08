using Microsoft.EntityFrameworkCore;
using GameStore.Api.Models;

namespace GameStore.Api.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) 
    : DbContext(options)
{
   public DbSet<Game> Games => Set<Game>();
    //    ^^^^^^^^^^^ means "a set of 'Game' objects". Each row in the table is one Game
    //                ^^^^^ is the property name (can be anything) but convention is to be plural of the tyhpe i think
    //                         ^^^^^^^^ asks EF Core for the set that stores Game objects. 

   public DbSet<Genre> Genres => Set<Genre>(); 
}