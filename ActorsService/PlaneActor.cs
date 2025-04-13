using Dapr.Actors.Runtime;
using System.Threading.Tasks;

namespace ActorsService
{
    public class PlaneActor : Actor, IPlaneActor
    {
        private const string StateKey = "PlaneCoordinates";

        public PlaneActor(ActorHost host) : base(host) { }

        public async Task<string> SaveCoordinatesAsync(PlaneCoordinates coordinates)
        {
            try
            {
                await StateManager.SetStateAsync<PlaneCoordinates>($"{StateKey}.{coordinates.PlaneId}", coordinates);
                return $"Success {StateKey}.{coordinates.PlaneId} {DateTime.Now}";
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
                return "Fail";
            }
        }

        public async Task<PlaneCoordinates?> GetCoordinatesAsync(string planeId)
        {
            return await StateManager.TryGetStateAsync<PlaneCoordinates>($"{StateKey}.{planeId}") is { HasValue: true } result
                ? result.Value
                : null;
        }
    }
}
