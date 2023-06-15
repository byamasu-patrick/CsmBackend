using AutoMapper;
using Shipping.API.Entities;
using Shipping.API.Models;
using Shipping.API.Entities;
using Shipping.API.Models;

namespace Shipping.API.Profiles
{
    public class LocationAddressProfile : Profile
    {
        public LocationAddressProfile()
        {
            CreateMap<CreateLocationAddressDto, LocationAddress>().ReverseMap();
        }
    }
}
