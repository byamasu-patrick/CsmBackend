using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entities;
using User.Application.Features.Queries.GetActivationToken;
using User.Application.Features.Queries.GetForgotPasswordToken;
using User.Application.Features.Queries.GetUserByEmail;
using User.Application.Features.Commands.ActivateUser;
using User.Application.Features.Commands.UpdateRefreshToken;
using User.Application.Features.Commands.InsertActivationToken;
using User.Application.Features.Commands.InsertForgotPasswordToken;

namespace User.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserData, UserVm>().ReverseMap();
            CreateMap<ActivationToken, InsertActivationTokenCommand>().ReverseMap();
            CreateMap<ForgotPasswordToken, InsertForgotPasswordTokenCommand>().ReverseMap();
            CreateMap<ActivationToken, ActivateUserCommand>().ReverseMap();
            CreateMap<ActivationToken, ActivationTokenVm>().ReverseMap();
            CreateMap<ForgotPasswordToken, ForgotPasswordTokenVm>().ReverseMap();

            CreateMap<UserData, UserDataVm>().ReverseMap();
            CreateMap<UserVm, UserDataVm>().ReverseMap();
            CreateMap<UserData, UserUpdateVm>().ReverseMap();
        }
    }
}
