using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Models;

namespace User.Application.Features.Commands.ResetPassword
{
    public class UpdateRefreshTokenCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public UpdateRefreshTokenCommandValidator()
        {
            RuleFor(p => p.Password)
                .NotEmpty()
               .NotNull()
               .MinimumLength(8)
               .MaximumLength(16)
               .Matches(AuthConstants.ALPHANUMERICAL).WithMessage("Password must contain alphanumeric characters");

            RuleFor(p => p.Token)
                .NotEmpty()
                .NotNull()
                .MaximumLength(255).WithMessage("{Salt} must not exceed 255 characters.");


        }
    }
}
