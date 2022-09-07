using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Features.Commands.InsertActivationToken;

namespace User.Application.Features.Commands.InsertForgotPasswordToken
{
    public class InsertForgotPasswordTokenCommandValidator : AbstractValidator<InsertActivationTokenCommand>
    {
        public InsertForgotPasswordTokenCommandValidator()
        {
            RuleFor(x => x.Token)
               .NotNull()
               .NotEmpty().WithMessage("{Token} must not be empty")
               .MaximumLength(255)
               .MinimumLength(2).WithMessage("{Token} must not be greater than 255 character");

            RuleFor(p => p.Email)
               .NotEmpty().WithMessage("{Email} is required.");

        }
    }
}
