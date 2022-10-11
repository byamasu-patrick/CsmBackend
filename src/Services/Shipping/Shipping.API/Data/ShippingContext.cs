using Shipping.API.Data.Interfaces;
using Shipping.API.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Payment.API.Data
{
    public class ShippingContext : IShippingContext
    {
        public ShippingContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));

            ShippingMethods = database.GetCollection<ShippingMethods>(configuration["DatabaseSettings:CollectionShippingMethods"]);
            ShippingAddresses = database.GetCollection<ShippingAddress>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            ShippingContextSeed.SeedData(ShippingAddresses, ShippingMethods);

        }

        public IMongoCollection<ShippingMethods> ShippingMethods { get; }
        public IMongoCollection<ShippingAddress> ShippingAddresses { get; }
    }
}
