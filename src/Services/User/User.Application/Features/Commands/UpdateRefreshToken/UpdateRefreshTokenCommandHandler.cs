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

namespace User.Application.Features.Commands.UpdateRefreshToken
{
    public class UpdateRefreshTokenCommandHandler : IRequestHandler<UpdateRefreshTokenCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        private readonly IMapper _mapper;


        private readonly ILogger<UpdateRefreshTokenCommandHandler> _logger;
        public UpdateRefreshTokenCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<UpdateRefreshTokenCommandHandler> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public async Task<bool> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
        {

                _userRepository.UpdateRefreshToken(
                    request.user,
                    request.RefreshToken,
                    request.TokenExpiryDate
                );

            // Send email to the Queue here using MassTransit and RabbitMQ

            return true;
        }
    }
}
