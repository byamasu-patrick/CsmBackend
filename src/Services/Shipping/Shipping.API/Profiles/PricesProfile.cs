using AutoMapper;
using Shipping.API.Entities;
using Shipping.API.Models;
using Shipping.API.Entities;
using Shipping.API.Models;

namespace Shipping.API.Profiles
{
    public class PricesProfile : Profile
    {
        public PricesProfile()
        {
            CreateMap<CreatePriceDto, Prices>().ReverseMap();
        }
    }
}
