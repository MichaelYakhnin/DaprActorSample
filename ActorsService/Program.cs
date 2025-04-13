using ActorsService;
using Dapr;
using Dapr.Actors;
using Dapr.Actors.Client;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddDapr();
// Add Dapr Actors
builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<PlaneActor>();
});

var app = builder.Build();

app.UseRouting();
app.UseCloudEvents();
app.MapActorsHandlers();
app.MapControllers();
app.MapSubscribeHandler();

// Map Dapr pub/sub route
app.MapPost("/plane-coordinates", [Topic("pubsub", "plane-coordinates")] async (PlaneCoordinates coordinates, [FromServices] IActorProxyFactory actorProxyFactory) =>
{
    var actorId = new ActorId("PlaneActor." + coordinates.PlaneId);
    var planeActor = actorProxyFactory.CreateActorProxy<IPlaneActor>(actorId, nameof(PlaneActor));
    var res = await planeActor.SaveCoordinatesAsync(coordinates);
   
    Console.WriteLine($"Got response: {res}");
});


await app.RunAsync();

