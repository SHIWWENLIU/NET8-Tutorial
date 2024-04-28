using MassTransit;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(x => {
    x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search", false));
    x.UsingRabbitMq((context,cfg) =>{
        //retry
        cfg.ReceiveEndpoint("search-auction-created", e =>
        {
            e.UseMessageRetry(r => r.Interval(5, 5));

            e.ConfigureConsumer<AuctionCreatedConsumer>(context);
        });

        cfg.ConfigureEndpoints(context);
        
    });
});

var app = builder.Build();



app.UseAuthorization();

app.MapControllers();

try{
    await DbInitializer.InitDb(app);
}
catch(Exception ex){
    Console.WriteLine(ex.ToString());
}

app.Run();
