using MediatR;
using Ordering.Application.Features.Commands.CheckoutOrder;

namespace Ordering.Application.Features.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommand : IRequest
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderingShoppingCartItem> Products { get; set; }

        // BillingAddress
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string District { get; set; }

        public string PhysicalAddress { get; set; }

        public string PaymentMethod { get; set; }

        public string OrderStatus { get; set; }
    }
}
