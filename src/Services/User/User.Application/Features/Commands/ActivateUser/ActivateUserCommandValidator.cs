using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Features.Commands.ActivateUser
{
    public class ActivateUserCommandValidator : AbstractValidator<ActivateUserCommand>
    {
        public ActivateUserCommandValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .NotNull()
                .NotEmpty();

            RuleFor(p => p.Token)
                .NotEmpty().WithMessage("{Token} is required.")
                .NotNull()
                .MaximumLength(255).WithMessage("{Token} must not exceed 255 characters.");

        }
    }
}
