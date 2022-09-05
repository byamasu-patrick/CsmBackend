using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Features.Queries.GetForgotPasswordToken
{
    public class GetForgotPasswordTokenQuery : IRequest<ForgotPasswordTokenVm>
    {
        public string ForgotPasswordToken { get; set; }
        public GetForgotPasswordTokenQuery(string forgotPasswordToken)
        {
            ForgotPasswordToken = forgotPasswordToken ?? throw new ArgumentNullException(nameof(forgotPasswordToken));
        }
    }
}
