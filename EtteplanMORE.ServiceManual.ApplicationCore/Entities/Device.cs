using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Entities
{
    public class Device
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string DeviceId { get; private set; }
        public string Name { get; private set; }
    }
}