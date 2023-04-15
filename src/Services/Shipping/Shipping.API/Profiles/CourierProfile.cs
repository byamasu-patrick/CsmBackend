using AutoMapper;
using Shipping.API.Entities;
using Shipping.API.Models;
using Shipping.API.Entities;
using Shipping.API.Models;

namespace Shipping.API.Profiles
{
    public class CourierProfile : Profile
    {
        public CourierProfile()
        {
            CreateMap<CreateCourierDto, Courier>().ReverseMap();
        }
    }
}

