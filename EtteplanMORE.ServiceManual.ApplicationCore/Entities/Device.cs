using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EtteplanMORE.ServiceManual.ApplicationCore.Entities
{
    public class Device
    {
        [BsonId]
        public string DeviceId { get; set; }
        public string Name { get; set; }
    }
}