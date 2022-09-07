using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Contracts.Persistence;
using User.Domain.Entities;

namespace User.Application.Features.Commands.ActivateUser
{
    public class ActivateUserCommandHandler : IRequestHandler<ActivateUserCommand, UserData>
    {
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;


        private readonly ILogger<ActivateUserCommandHandler> _logger;
        public ActivateUserCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<ActivateUserCommandHandler> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<UserData> Handle(ActivateUserCommand request, CancellationToken cancellationToken)
        {
            var activationToken = _mapper.Map<ActivationToken>(request);

            var activatedUser = await _userRepository.ActivateUser(activationToken);

            _logger.LogInformation($"User email for {activatedUser.DisplayName } is successfully activated.");

            return activatedUser;
        }
    }
}
