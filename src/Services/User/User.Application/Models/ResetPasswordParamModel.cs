using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Models
{
    public class ResetPasswordParamModel
    {
        public string Password { get; set; }
        public string Token { get; set; }
    }
    public class ResetPasswordParamModelValidator : AbstractValidator<ResetPasswordParamModel>
    {
        public ResetPasswordParamModelValidator()
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .NotNull()
                .MinimumLength(8)
                .MaximumLength(16)
                .Matches(AuthConstants.ALPHANUMERICAL).WithMessage("Password must contain alphanumeric characters");

            RuleFor(x => x.Token)
                .NotNull()
                .NotEmpty();
        }
    }
}
