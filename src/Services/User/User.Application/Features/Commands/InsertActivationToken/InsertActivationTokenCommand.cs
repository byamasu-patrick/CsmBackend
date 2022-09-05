using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Features.Commands.InsertActivationToken
{
    public class InsertActivationTokenCommand : IRequest<bool>
    {
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
