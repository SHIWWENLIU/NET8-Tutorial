
using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app){
        //create scope for the service
        using var scope = app.Services.CreateScope();

        SeedData(scope.ServiceProvider.GetService<DataContext>());
    }

    private static void SeedData(DataContext context)
    {
        context.Database.Migrate();

        if(context.SuperHeros.Any()){
            return;
        }

        var SuperHeros = new List<SuperHero>(){
            new SuperHero(){
                Id=1,
                Name = "spiderman",
                FirstName="peter",
                LastName="Parker",
                Place="new york"
            },
            new SuperHero(){
                Id=2,
                Name = "iron man",
                FirstName="tony",
                LastName="stark",
                Place="new york"
            }
        };

        context.AddRange(SuperHeros);

        context.SaveChanges();
    }
}
