using AutoMapper;
using Shipping.API.Entities;
using Shipping.API.Models;
using Shipping.API.Entities;
using Shipping.API.Models;

namespace Shipping.API.Profiles
{
    public class ReceiverProfile : Profile
    {
        public ReceiverProfile()
        {
            CreateMap<CreateReceiverDto, Receiver>().ReverseMap();
        }
    }
}
