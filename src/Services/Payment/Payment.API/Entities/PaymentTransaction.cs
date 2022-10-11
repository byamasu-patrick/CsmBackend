using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

namespace Payment.API.Entities
{
    public class PaymentTransaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string TransactionId { get; set; }
        [Required]
        public long TotalAmount { get; set; }
        [Required]
        public bool IsApproved { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public IList<Product> Products { get; set; }
        [Required]
        [BsonElement]
        public DateTime CreatedAt { get; set; }
    }
    public class Product
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Summary { get; set; }
        public long Price { get; set; }
        public double Discount { get; set; }
        public string ImageUrl { get; set; }
        public int Items { get; set; }
        public string UserId { get; set; }
    }
}
