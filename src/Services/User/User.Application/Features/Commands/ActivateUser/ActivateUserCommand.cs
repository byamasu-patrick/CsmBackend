using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entities;

namespace User.Application.Features.Commands.ActivateUser
{
    public class ActivateUserCommand : IRequest<UserData>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
