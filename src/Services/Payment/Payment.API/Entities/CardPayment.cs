using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;


namespace Payment.API.Entities
{
    public class CardPayment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string UserId { get; set; }
        // Billing Address
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string EmailAddress { get; set; }
        [Required]
        public string AddressLine { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string ZipCode { get; set; }
        [Required]

        // Payment
        public string CardName { get; set; }
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string Expiration { get; set; }
        [Required]
        public string CVV { get; set; }
        [Required]
        public bool Default { get; set; }
        [Required]
        [BsonElement]
        public DateTime CreatedAt { get; set; }
    }
}
