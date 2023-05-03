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
            var products = new List<Product>();
            //products.Add("63171fb70f38058887656184");
            var product = new Product
            {
                id = 1,
                Quantity = 2,
                Color = "Red",
                Price = 1000.99m,
                SubTotal = 2001.98m,
                ProductId = "123",
                ProductName = "Totenhamm Hotspurs Jersy"
            };

            products.Add(product);


            return new List<Order>
            {
                new Order() { UserName = "Patrick", FirstName = "Byamasu", LastName = "Patrick",
                    EmailAddress = "ptrckbyamasu@gmail.com", OrderStatus = "Processing",
                    PhysicalAddress = "Zolozolo", TotalPrice = 350,
                    Products=products, PaymentMethod = "AirtelMoney",
                    CourierName = "CTS",
                    NationalId = "123",PhoneNumber = "+265882751146" 
                }
            };
        }
    }
}
