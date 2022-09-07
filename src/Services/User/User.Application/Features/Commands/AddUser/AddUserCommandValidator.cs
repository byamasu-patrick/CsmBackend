using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Models;

namespace User.Application.Features.Commands.AddUser
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator()
        {
            RuleFor(x => x.Password)
               .NotEmpty()
               .NotNull()
               .MinimumLength(8)
               .MaximumLength(16)
               .Matches(AuthConstants.ALPHANUMERICAL).WithMessage("Password must contain alphanumeric characters");

            RuleFor(x => x.Email)
                .EmailAddress()
                .NotNull()
                .NotEmpty();

            RuleFor(x => x.FirstName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(255)
                .MinimumLength(2);

            RuleFor(x => x.LastName)
                .NotNull()
                .NotEmpty()
                .MaximumLength(255)
                .MinimumLength(2);

        }
    }
}
