using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Contracts.Persistence;
using User.Application.Enums;

namespace User.Application.Features.Queries.GetForgotPasswordToken
{
    public class GetForgotPasswordTokenQueryHandler : IRequestHandler<GetForgotPasswordTokenQuery, ForgotPasswordTokenVm>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public GetForgotPasswordTokenQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async Task<ForgotPasswordTokenVm> Handle(GetForgotPasswordTokenQuery request, CancellationToken cancellationToken)
        {
            var activationTokenData = await _userRepository.ValidateApplicationToken(request.ForgotPasswordToken, TokenType.ResetPassword);

            return _mapper.Map<ForgotPasswordTokenVm>(activationTokenData);
        }
    }
}
