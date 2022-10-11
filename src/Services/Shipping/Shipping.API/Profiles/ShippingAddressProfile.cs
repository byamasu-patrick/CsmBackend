using AutoMapper;
using Shipping.API.Entities;
using Shipping.API.Models;
using Shipping.API.Entities;
using Shipping.API.Models;

namespace Shipping.API.Profiles
{
    public class ShippingAddressProfile : Profile
    {
        public ShippingAddressProfile()
        {
            CreateMap<CreateShippingAddresseDto, ShippingAddress>().ReverseMap();
        }
    }
}
