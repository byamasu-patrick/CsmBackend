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

namespace User.Application.Features.Commands.InsertActivationToken
{
    public class AddUserCommandHandler : IRequestHandler<InsertActivationTokenCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;


        private readonly ILogger<AddUserCommandHandler> _logger;
        public AddUserCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<AddUserCommandHandler> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> Handle(InsertActivationTokenCommand request, CancellationToken cancellationToken)
        {
            var activationToken = _mapper.Map<ActivationToken>(request);

            await _userRepository.InsertActivationToken(activationToken);

            // Send email to the Queue here using MassTransit and RabbitMQ

            return true;
        }
    }
}
