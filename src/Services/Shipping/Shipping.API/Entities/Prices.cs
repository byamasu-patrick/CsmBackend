using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.API.Entities
{
    public class Prices
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string KillogramRange { get; set; }
        [Required]
        public string price { get; set; }

        [ForeignKey("Location")]
        public string LocationId { get; set; }
        public LocationAddress Location { get; set; }
    }
}
