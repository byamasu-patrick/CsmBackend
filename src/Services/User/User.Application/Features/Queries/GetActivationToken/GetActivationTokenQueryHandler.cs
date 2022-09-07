using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Contracts.Persistence;

namespace User.Application.Features.Queries.GetActivationToken
{
    public class GetForgotPasswordTokenQueryHandler : IRequestHandler<GetActivationTokenQuery, ActivationTokenVm>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetForgotPasswordTokenQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<ActivationTokenVm> Handle(GetActivationTokenQuery request, CancellationToken cancellationToken)
        {
            var activationTokenData = await _userRepository.GetActivationToken(request.ActivationToken);

            return _mapper.Map<ActivationTokenVm>(activationTokenData);
        }
    }
}
