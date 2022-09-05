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

namespace User.Application.Features.Commands.AddUser
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, CreationResults<UserData>>
    {
        private readonly IUserRepository _userRepository;

        private readonly IPasswordService _passowrdService;

        private readonly IEmailService _emailService;

        private readonly IMapper _mapper;


        private readonly ILogger<AddUserCommandHandler> _logger;
        public AddUserCommandHandler(IUserRepository userRepository, IMapper mapper, ILogger<AddUserCommandHandler> logger, IPasswordService passwordService, IEmailService emailService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _passowrdService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _emailService = emailService;
        }
        public async Task<CreationResults<UserData>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var userExist = await _userRepository.GetUserByEmail(request.Email);

            if (userExist != null)
            {
                return new CreationResults<UserData>
                {
                    Success = false,
                    Message = "User already exists",
                    Status = 400,
                    Data = null
                };                   
            }

            (string hash, string salt) = _passowrdService.HashPassword(request.Password);

            // Send email to the Queue here using MassTransit and RabbitMQ
            var userData = new UserData
            {
                Email = request.Email,
                IsActive = true,
                DisplayName = request.FirstName +" "+ request.LastName,
                EmailConfirmed = false,
                Password = hash,
                Salt = salt,
                UserTypeId = (int) AccountType.FreeUser,
                Profile = new UserProfile
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    CreatedAt = DateTime.UtcNow,
                },
                CreatedAt = DateTime.UtcNow
            };

            //_emailService.SendActivationMail(userData.Email, userData.DisplayName,
            //    userData.);
            // Create actual account

            var user = await _userRepository.AddAsync(userData);

            return new CreationResults<UserData>
            {
                Success = true,
                Message = "Account Created Successfully",
                Status = 200,
                Data = user
            };
        }
    }
}
