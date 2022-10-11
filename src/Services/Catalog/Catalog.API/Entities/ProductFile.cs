using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Catalog.API.Entities
{
    public class ProductFile
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get ; set; }
        public string Url { get; set; }
        public string FileData { get; set; }
        public string MimeType { get; set; }
        public string Size { get; set; }
        public string ProductId { get; set; }
        [BsonElement]
        public DateTime CreatedAt { get; set; }
    }
}
