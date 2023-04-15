using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.API.Entities
{
    public class LocationAddress
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]

        public string Id { get; set; }
        [Required]
        public string Source { get; set; }
        [Required]
        public string Destination { get; set; }

        public ICollection<Prices> prices { get; set; }

        [ForeignKey("Courier")]
        public string CourierId { get; set; }
        public Courier Courier { get; set; }

    }
}
