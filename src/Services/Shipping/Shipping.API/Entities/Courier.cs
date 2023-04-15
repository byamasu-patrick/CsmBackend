using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shipping.API.Entities
{
    public class Courier
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set;  }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Contact1 { get; set; }
        [Required]
        public string Contact2 { get; set; }
        [Required]

        public string HeadOfficeLocation { get; set; }

        public ICollection<LocationAddress> LocationAddresses { get; set; }
        public DateTime CreatedDate { get; set; }

    }
}
