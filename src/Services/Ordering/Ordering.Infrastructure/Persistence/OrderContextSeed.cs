using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreconfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed database associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }

        private static IEnumerable<Order> GetPreconfiguredOrders()
        {
            var products = new List<string>();
            products.Add("63171fb70f38058887656184");
            return new List<Order>
            {
                new Order() { UserName = "Patrick", FirstName = "Byamasu", LastName = "Patrick", EmailAddress = "ptrckbyamasu@gmail.com", AddressLine = "Lilongwe, Area 47, Sector 4", Country = "Malawi", TotalPrice = 350, Products=products, CVV = "520",
                    CardName = "Byamasu Patrick", CardNumber = "4598675434529988", PaymentMethod = 1, ZipCode = "00265", Expiration = "2023", State = "Lilongwe"
                }
            };
        }
    }
}
