using AmpMailer.Contracts;
using AutoMapper;
using EventBus.Messages.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmpMailer.Profiles
{
    public class EmailEventProfile : Profile
    {
        public EmailEventProfile()
        {
            CreateMap<EmailEvent, EmailEventData>().ReverseMap();
        }
    }
}
