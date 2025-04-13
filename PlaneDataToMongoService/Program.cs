using Dapr.Actors;
using Dapr.Actors.Client;
using MongoDB.Driver;
using PlaneDataToMongoService;
using System.Numerics;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// MongoDB Configuration
var mongoConnectionString = "mongodb://localhost:27017"; // Replace with your MongoDB connection string
var databaseName = "PlaneDataDB";
var collectionName = "PlaneCoordinates";

var mongoClient = new MongoClient(mongoConnectionString);
var database = mongoClient.GetDatabase(databaseName);
var collection = database.GetCollection<PlaneCoordinatesDB>(collectionName);

// Periodically fetch data from actors and update MongoDB
app.Lifetime.ApplicationStarted.Register(async () =>
{
    while (true)
    {
        try
        {
            // Simulate fetching data for 100 planes
            for (int i = 1; i <= 100; i++)
            {
                var actorId = new ActorId($"PlaneActor.Plane{i}");
                var planeActor = ActorProxy.Create<IPlaneActor>(actorId, "PlaneActor");

                // Fetch plane data from the actor
                var planeData = await planeActor.GetCoordinatesAsync($"Plane{i}");
                if (planeData != null)
                {
                    // Upsert data into MongoDB
                    var filter = Builders<PlaneCoordinatesDB>.Filter.Eq(p => p.PlaneId, planeData.PlaneId);
                    await collection.ReplaceOneAsync(filter, new PlaneCoordinatesDB(planeData.PlaneId,planeData.Latitude,planeData.Longitude,planeData.Altitude),
                        new ReplaceOptions { IsUpsert = true });

                    Console.WriteLine($"Updated MongoDB with data: {planeData}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        await Task.Delay(5000); // Wait 5 seconds before the next update
    }
});

app.Run();
