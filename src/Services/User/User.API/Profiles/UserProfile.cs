using AutoMapper;
using EventBus.Messages.Events;
using User.API.Models;
using User.Application.Contracts.Infrastructure;
using User.Application.Features.Queries.GetShops;

namespace User.API.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserInfos, UserVmShops>().ReverseMap();
        }   
    }
}
