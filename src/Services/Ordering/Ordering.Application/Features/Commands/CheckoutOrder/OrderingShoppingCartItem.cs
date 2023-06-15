using Ordering.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Ordering.Application.Features.Commands.CheckoutOrder
{
    public class OrderingShoppingCartItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
        public string Color { get; set; }
        public decimal Weight { get; set; }
    }
}
