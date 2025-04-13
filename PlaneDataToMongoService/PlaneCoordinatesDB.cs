using MongoDB.Bson.Serialization.Attributes;

namespace PlaneDataToMongoService
{
    public record PlaneCoordinatesDB(
       [property: BsonId] string PlaneId,
       [property: BsonElement("latitude")] double Latitude,
       [property: BsonElement("longitude")] double Longitude,
       [property: BsonElement("altitude")] double Altitude)
    {
        public override string ToString()
        {
            return $"PlaneId: {PlaneId}, Latitude: {Latitude:F6}, Longitude: {Longitude:F6}, Altitude: {Altitude} ft";
        }
    }
}
