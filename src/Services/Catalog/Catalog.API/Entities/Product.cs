﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.API.Entities
{
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
        public string Category { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string ImageFile { get; set; }
        public decimal Price { get; set; }
        public decimal Weight { get; set; }
        public int ItemsInStock { get; set; }
        //public string Color { get; set; }
        public string UserId { get; set; }
        //public IList<ProductReview> Reviews { get; set; }
        [BsonElement]
        public DateTime CreatedAt { get; set; }
    }
}
