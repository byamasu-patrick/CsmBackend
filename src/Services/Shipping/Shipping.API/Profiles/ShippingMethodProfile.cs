using AutoMapper;
using Shipping.API.Entities;
using Shipping.API.Models;

namespace Shipping.API.Profiles
{
    public class ShippingMethodProfile : Profile
    {
        public ShippingMethodProfile()
        {
            CreateMap<CreateShippingMethodDto, ShippingMethods>().ReverseMap();
        }
    }
}
