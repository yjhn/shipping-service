using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Repositories.Entities
{
    public class Package
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? _id { get; set; }

        [BsonElement]
        public string Name { get; set; }

        [BsonElement]
        public string Description { get; set; }

        [BsonElement]
        public PostMachine SourceMachine { get; set; }

        [BsonElement]
        public PostMachine DestinationMachine { get; set; }

    }
}
