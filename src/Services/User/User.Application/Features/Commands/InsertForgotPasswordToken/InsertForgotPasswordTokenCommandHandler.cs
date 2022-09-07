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

namespace User.Application.Features.Commands.InsertForgotPasswordToken
{
    public class InsertForgotPasswordTokenCommandHandler : IRequestHandler<InsertForgotPasswordTokenCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;


        private readonly ILogger<InsertForgotPasswordTokenCommandHandler> _logger;
        public InsertForgotPasswordTokenCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<InsertForgotPasswordTokenCommandHandler> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> Handle(InsertForgotPasswordTokenCommand request, CancellationToken cancellationToken)
        {
            var activationToken = _mapper.Map<ForgotPasswordToken>(request);

            await _userRepository.InsertForgotPasswordToken(activationToken);

            // Send email to the Queue here using MassTransit and RabbitMQ

            return true;
        }
    }
}
