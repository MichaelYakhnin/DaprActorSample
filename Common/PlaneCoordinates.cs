using System.Runtime.Serialization;

public class PlaneCoordinates
{
    public string PlaneId { get; set; } = default!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Altitude { get; set; }

    public override string ToString()
    {
        return $"PlaneId: {PlaneId}, Latitude: {Latitude:F6}, Longitude: {Longitude:F6}, Altitude: {Altitude} ft";
    }
}

