using AutoMapper;
using Basket.API.Entities;
using EventBus.Messages.Events;
//using EventBus.Messages.Events;


namespace Basket.API.Mapper
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketCheckout, BasketCheckoutEvent>().ReverseMap();
            CreateMap<BasketProduct, ShoppingCartItem>().ReverseMap();
        }
    }
}
