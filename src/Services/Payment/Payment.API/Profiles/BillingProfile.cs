using AutoMapper;
using Payment.API.Entities;
using Payment.API.Models;

namespace Payment.API.Profiles
{
    public class BillingProfile : Profile
    {
        public BillingProfile()
        {
            CreateMap<CardPayment, CardPaymentDto>().ReverseMap();
        }
    }
}
