using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Shipping.API.Entities
{
    public class ShippingAddress
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string MapKey { get; set; }
        [Required]
        public string Geocode { get; set; }
        [Required]
        public string Street1 { get; set; }
        [Required]
        public string Street2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string PoBox { get; set; }
        [Required]
        public string PoBoxAllowed { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string PostalCodeType { get; set; }
        [Required]
        public string CustomerId { get; set; }
        [Required]
        [BsonElement]
        public DateTime CreatedAt { get; set; }

    }
}
