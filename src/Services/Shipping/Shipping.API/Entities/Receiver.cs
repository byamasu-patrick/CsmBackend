
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.API.Entities
{
    public class Receiver
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; } 
        [Required]
        public string Name { get; set; }
        [Required]
        public string Contact { get; set; }
        [Required]
        public string NationalId { get; set; }
        [Required]
        public string Killograms { get; set; } 

        public string Courier { get; set; } 

        public string Location { get; set; }

        public string Price { get; set; }
    }
}
