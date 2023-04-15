using AutoMapper;
using EventBus.Messages.Events;
using Ordering.Application.Features.Commands.CheckoutOrder;

namespace Ordering.API.Mapper
{
    public class OrderingProfile : Profile
	{
		public OrderingProfile()
		{
			CreateMap<CheckoutOrderCommand, BasketCheckoutEvent>().ReverseMap();
            CreateMap<OrderingShoppingCartItem, ShoppingCartItem>().ReverseMap();
        }
	}
}
