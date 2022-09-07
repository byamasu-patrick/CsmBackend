using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Domain.Entities;

namespace User.Application.Features.Commands.AddUser
{
    public class AddUserCommand : IRequest<CreationResults<UserData>>
    {
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Password { get; set; }
        public string? AccountType { get; set; }
    }
}
