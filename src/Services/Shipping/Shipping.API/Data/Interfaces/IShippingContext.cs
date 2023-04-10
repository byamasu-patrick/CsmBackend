using Shipping.API.Entities;
using MongoDB.Driver;
using Shipping.API.Entities;

namespace Shipping.API.Data.Interfaces
{
    public interface IShippingContext
    {
        IMongoCollection<ShippingAddress> ShippingAddresses { get; }
        IMongoCollection<ShippingMethods> ShippingMethods { get; }
        IMongoCollection<Courier> Couriers { get; }

        IMongoCollection<LocationAddress> Locations { get; }

        IMongoCollection<Receiver> Receivers { get; }

        IMongoCollection<Prices> Prices { get; }
    }
}
