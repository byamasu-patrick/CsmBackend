using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Features.Queries.GetUserByEmail
{
    public class GetUserByEmailQuery : IRequest<UserVm>
    {
        public string Email { get; set; }
        public GetUserByEmailQuery(string email)
        {
            Email = email ?? throw new ArgumentNullException(nameof(email));
        }
    }
}
