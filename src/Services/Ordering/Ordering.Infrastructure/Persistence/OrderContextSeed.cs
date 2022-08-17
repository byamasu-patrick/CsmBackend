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
            return new List<Order>
            {
                new Order() { UserName = "Patrick", FirstName = "Byamasu", LastName = "Patrick", EmailAddress = "ptrckbyamasu@gmail.com", AddressLine = "Lilongwe, Area 47, Sector 4", Country = "Malawi", TotalPrice = 350, CVV = "520",
                    CardName = "Byamasu Patrick", CardNumber = "4598675434529988", PaymentMethod = 1, ZipCode = "00265", Expiration = "2023", State = "Lilongwe"
                }
            };
        }
    }
}
