using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Shipping.API.Entities
{
    public class ShippingMethods
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string Carrier { get; set; }
        [Required]
        public string Method { get; set; }
        [Required]
        public string AverageShippingTime { get; set; }
        [Required]
        public string Price { get; set; }
        [Required]
        public string PriceCurrency { get; set; }
        [Required]
        public double WeightLimit { get; set; }
        [Required]
        public string Restrictions { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int PoBoxAllowed { get; set; }
        [Required]
        public int SignatureRequired { get; set; }
        [Required]
        public int SaturdayDelivery { get; set; }
        [Required]
        public int InternationalDelivery { get; set; }
        [Required]
        public int SizeLimit { get; set; }
        [Required]
        public string PackagingType { get; set; }
        [Required]
        [BsonElement]
        public DateTime CreatedAt { get; set; }
    }
}
