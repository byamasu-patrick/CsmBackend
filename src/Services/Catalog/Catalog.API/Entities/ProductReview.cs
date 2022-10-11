using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Catalog.API.Entities
{
    public class ProductReview
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string CustomerId { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        [BsonElement]
        public DateTime CreatedAt { get; set; }
    }
}
