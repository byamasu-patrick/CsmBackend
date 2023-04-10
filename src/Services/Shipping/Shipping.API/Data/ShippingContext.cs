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
            Couriers = database.GetCollection<Courier>(configuration["DatabaseSettings:CollectionCourier"]);
            Receivers = database.GetCollection<Receiver>(configuration["DatabaseSettings:CollectionReceiver"]);
            Locations = database.GetCollection<LocationAddress>(configuration["DatabaseSettings:CollectionLocationAddress"]);
            Prices = database.GetCollection<Prices>(configuration["DatabaseSettings:CollectionPrices"]);
            ShippingAddresses = database.GetCollection<ShippingAddress>(configuration.GetValue<string>("DatabaseSettings:CollectionName"));
            ShippingContextSeed.SeedData(ShippingAddresses, ShippingMethods, Prices, Couriers, Receivers, Locations);

        }

        public IMongoCollection<ShippingMethods> ShippingMethods { get; }
        public IMongoCollection<ShippingAddress> ShippingAddresses { get; }

        public IMongoCollection<Courier> Couriers { get;  }

        public IMongoCollection<LocationAddress> Locations { get;  }

        public IMongoCollection<Receiver> Receivers { get;  }

        public IMongoCollection<Prices> Prices { get; }
    }
}
