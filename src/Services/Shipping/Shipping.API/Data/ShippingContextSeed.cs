using Shipping.API.Entities;
using MongoDB.Driver;
using Shipping.API.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Payment.API.Data
{
    public class ShippingContextSeed
    {
        public static void SeedData(IMongoCollection<ShippingAddress> shippingAddress, IMongoCollection<ShippingMethods> shippingMethods)
        {
            
            //Console.WriteLine("Arrived");
            bool existProduct = shippingMethods.Find(p => true).Any();
            if (!existProduct)
            {
                shippingMethods.InsertManyAsync(GetPreconfiguredShippingMethods());
                shippingAddress.InsertOneAsync(GetPreconfiguredShippingAddress());
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
    }
}
