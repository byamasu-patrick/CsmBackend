using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Contracts.Infrastructure;
using User.Application.Contracts.Persistence;
using User.Application.Enums;
using User.Domain.Entities;

namespace User.Application.Features.Commands.ResetPassword
{
    public class UpdateRefreshTokenCommandHandler : IRequestHandler<ResetPasswordCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        private readonly IPasswordService _passowrdService;

        private readonly IMapper _mapper;


        private readonly ILogger<UpdateRefreshTokenCommandHandler> _logger;
        public UpdateRefreshTokenCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<UpdateRefreshTokenCommandHandler> logger,
            IPasswordService passwordService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _passowrdService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
        }
        public async Task<bool> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var resetToken = await _userRepository.ValidateApplicationToken(request.Token, TokenType.ResetPassword);

            (string hash, string salt) = _passowrdService.HashPassword(request.Password);


            await _userRepository.ResetPassword(
                    resetToken.Email,
                    hash,
                    salt
                );

            // Send email to the Queue here using MassTransit and RabbitMQ

            return true;
        }
    }
}
