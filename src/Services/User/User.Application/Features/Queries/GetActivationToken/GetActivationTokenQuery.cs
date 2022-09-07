using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Features.Queries.GetActivationToken
{
    public class GetActivationTokenQuery : IRequest<ActivationTokenVm>
    {
        public string ActivationToken { get; set; }
        public GetActivationTokenQuery(string activationToken)
        {
            ActivationToken = activationToken ?? throw new ArgumentNullException(nameof(activationToken));
        }
    }
}
