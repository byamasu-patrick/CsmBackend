using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Features.Commands.UpdateRefreshToken
{
    public class UpdateRefreshTokenCommandValidator : AbstractValidator<UpdateRefreshTokenCommand>
    {
        public UpdateRefreshTokenCommandValidator()
        {
            RuleFor(p => p.RefreshToken)
                .NotEmpty().WithMessage("{Token} is required.")
                .NotNull()
                .MaximumLength(255).WithMessage("{Token} must not exceed 255 characters.");

            RuleFor(p => p.TokenExpiryDate)
                .NotEmpty().WithMessage("{TokenExpiryDate} is required.")
                .NotNull();

            RuleFor(p => p.user)
               .NotNull().WithMessage("User cannot be null.");

        }
    }
}
