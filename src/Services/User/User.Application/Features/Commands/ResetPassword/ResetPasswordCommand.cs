using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Features.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<bool>
    {
        public string Password { get; set; }
        public string Token { get; set; }
    }
}
