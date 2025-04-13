using Dapr.Client;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var pubsubName = "pubsub";
var topicName = "plane-coordinates";

var random = new Random();
var planes = new List<Plane>();

// Initialize 100 planes with random starting positions
for (int i = 1; i <= 100; i++)
{
    planes.Add(new Plane($"Plane{i}",
        random.NextDouble() * 180 - 90,  // Latitude: -90 to 90
        random.NextDouble() * 360 - 180, // Longitude: -180 to 180
        random.Next(5000, 40000)));      // Altitude: 5000 to 40000 feet
}

// Simulate plane movement and publish data every 5 seconds
app.Lifetime.ApplicationStarted.Register(async () =>
{
    using var client = new DaprClientBuilder().Build();
    await Task.Delay(5000);
while (true)
{
    foreach (var plane in planes)
        {
            plane.SimulateMovement();
            var planeData = new PlaneCoordinates(plane.Id, plane.Latitude, plane.Longitude, plane.Altitude);
            await client.PublishEventAsync(pubsubName, topicName, planeData);
            Console.WriteLine($"Published: {JsonSerializer.Serialize(planeData)}");
        }

        await Task.Delay(5000); // Wait 5 seconds before the next update
    }
});

app.Run();

public record PlaneCoordinates(string PlaneId, double Latitude, double Longitude, double Altitude);

public class Plane
{
    public string Id { get; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public double Altitude { get; private set; }

    public Plane(string id, double latitude, double longitude, double altitude)
    {
        Id = id;
        Latitude = latitude;
        Longitude = longitude;
        Altitude = altitude;
    }

    public void SimulateMovement()
    {
        // Simulate random movement
        Latitude += (new Random().NextDouble() - 0.5) * 0.1; // Small random change
        Longitude += (new Random().NextDouble() - 0.5) * 0.1;
        Altitude += (new Random().Next(-100, 100)); // Random altitude change
    }
}
