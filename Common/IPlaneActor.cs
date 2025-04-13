using Dapr.Actors;
using System.Threading.Tasks;


public interface IPlaneActor : IActor
{
    Task<string> SaveCoordinatesAsync(PlaneCoordinates coordinates);
    Task<PlaneCoordinates?> GetCoordinatesAsync(string planeId);
}


