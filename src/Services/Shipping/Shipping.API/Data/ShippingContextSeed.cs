using Shipping.API.Entities;
using MongoDB.Driver;
using Shipping.API.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Payment.API.Data
{
    public class ShippingContextSeed
    {
        public static void SeedData(IMongoCollection<ShippingAddress> shippingAddress,
            IMongoCollection<ShippingMethods> shippingMethods,
            IMongoCollection<Prices> Prices,
            IMongoCollection<Courier> Courier,
            IMongoCollection<Receiver> Receiver,
            IMongoCollection<LocationAddress> Location)
        {
            
            //Console.WriteLine("Arrived");
            bool existProduct = shippingMethods.Find(p => true).Any();
            if (!existProduct)
            {
                shippingMethods.InsertManyAsync(GetPreconfiguredShippingMethods());
                shippingAddress.InsertOneAsync(GetPreconfiguredShippingAddress());
            }
            bool exitCourier = Courier.Find(p => true).Any();
            if (!exitCourier)
            {
                Courier.InsertManyAsync(GetPreconfiguredCourier());
            }
            bool existReceiver = Receiver.Find(p => true).Any();
            if (! existReceiver)
            {
                Receiver.InsertManyAsync(GetPreconfiguredReceiver());
            }
            bool existLocation = Location.Find(p => true).Any();
            if (!existReceiver)
            {
                Location.InsertManyAsync(GetPreconfiguredLocation());
            }
            bool existPrices = Prices.Find(p => true).Any();
            if (!existPrices)
            {
                Prices.InsertManyAsync(GetPreconfiguredPrices());
            }
        }
        private static IEnumerable<ShippingMethods> GetPreconfiguredShippingMethods()
        {
            return new List<ShippingMethods>()
            {
               new ShippingMethods()
               {

               }
            };
        }

        private static ShippingAddress GetPreconfiguredShippingAddress()
        {
            return new ShippingAddress()
            {
                
            };
        }

        private static IEnumerable<Courier> GetPreconfiguredCourier()
        {
            return new List<Courier>()
    {
        new Courier()
        {
            // Set properties with valid data
             Name = "Ankolo",
             Contact1 = "0991644383",
             Contact2 = "08812345555",
             HeadOfficeLocation = "Blantyre",
             CreatedDate = DateTime.Now
        },
        new Courier()
        {
             Name = "Speed",
             Contact1 = "0991644383",
             Contact2 = "08812345555",
             HeadOfficeLocation = "Lilongwe",
             CreatedDate = DateTime.Now

        }
        
    };
        }
        private static IEnumerable<Receiver> GetPreconfiguredReceiver()
        {
            return new List<Receiver>()
    {
        new Receiver()
        {
            // Set properties with valid data
            UserName = "austinthope12@gmail.com",
             Name = "Austin Thope",
             Contact = "0991644383",
              NationalId= "0W324FD3",
             Killograms = "2",
             Courier = "Ankolo",
             Location = "Blantyre",
             Price = "1500"
        },
        new Receiver()
        {
             UserName = "akash@gmailc.com",
             Name = "Donald Phiri",
             Contact = "0991644383",
              NationalId= "0W324FD3",
             Killograms = "4",
             Courier = "CTS",
             Location = "Lilongwe",
             Price = "1500"

        }
 
    };
        }

        private static IEnumerable<LocationAddress> GetPreconfiguredLocation()
        {
            return new List<LocationAddress>()
    {
        new LocationAddress()
        {
            // Set properties with valid data
            Source = "Mzuzu",
            Destination = "Blantyre"
        },
        new LocationAddress()
        {

            Source = "Mzuzu",
            Destination = "Lilongwe"
        }

    };
        }
        private static IEnumerable<Prices> GetPreconfiguredPrices()
        {
            return new List<Prices>()
    {
        new Prices()
        {
            // Set properties with valid data
            FromKg= 0,
            ToKg = 2,
            price = 1500
        },
        new Prices()
        {

            FromKg = 3,
            ToKg = 5,
            price = 2500
        }

    };
        }

    }
}
