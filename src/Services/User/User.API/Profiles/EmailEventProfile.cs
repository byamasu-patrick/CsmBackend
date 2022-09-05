using AutoMapper;
using EventBus.Messages.Events;
using User.Application.Contracts.Infrastructure;

namespace User.API.Profiles
{
    public class EmailEventProfile : Profile
    {
        public EmailEventProfile()
        {
            CreateMap<EmailEvent, EmailEventStructure>();
            CreateMap<EmailEventStructure, EmailEvent>();
        }
    }
}
