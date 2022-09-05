using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entities;

namespace User.Application.Features.Commands.UpdateRefreshToken
{
    public class UpdateRefreshTokenCommand : IRequest<bool>
    {
        public string RefreshToken { get; set; }
        public DateTime TokenExpiryDate { get; set; }
        public UserData user { get; set; }

    }
}
