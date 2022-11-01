using AutoMapper;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using User.API.Models;
using User.Application.Contracts.Infrastructure;
using User.Application.Contracts.Persistence;
using User.Application.Enums;
using User.Application.Features.Commands.ActivateUser;
using User.Application.Features.Commands.AddUser;
using User.Application.Features.Commands.InsertActivationToken;
using User.Application.Features.Commands.InsertForgotPasswordToken;
using User.Application.Features.Commands.ResetPassword;
using User.Application.Features.Commands.UpdateRefreshToken;
using User.Application.Features.Queries.GetActivationToken;
using User.Application.Features.Queries.GetForgotPasswordToken;
using User.Application.Features.Queries.GetShops;
using User.Application.Features.Queries.GetUserByEmail;
using User.Application.Models;
using User.Domain.Common.Models;
using User.Domain.Entities;

namespace User.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly IMediator _mediator;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        private readonly IPublishEndpoint _publishEndpoint;

        public AuthController(IMediator mediator, IPasswordService passwordService, ITokenService tokenService,
            IMapper mapper, IEmailService emailService, ILogger<AuthController> logger, 
            IPublishEndpoint publishEndpoint)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _passwordService = passwordService ?? throw new ArgumentNullException(nameof(passwordService));
            _tokenService = tokenService;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); 
            _emailService = emailService;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _publishEndpoint = publishEndpoint ?? throw new ArgumentNullException(nameof(publishEndpoint));
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(UserVm), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ApiResponse<TokenResponseModel>>> Login([FromBody] LoginParamModel loginParamModel)
        {
            var query = new GetUserByEmailQuery(loginParamModel.Email);
            var user = await _mediator.Send(query);

            if (user != null && user.Password is not null && _passwordService.ComparePasswordWithHash(user.Password, user.Salt, loginParamModel.Password))
            {
                if (!user.EmailConfirmed)
                {
                    return BadRequest(new ApiResponse("Email is not confirmed") { Code = (int)AuthError.EmailNotConfirmed });
                }
                var authClaims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Name, user.Profile.FirstName),
                    new Claim(JwtRegisteredClaimNames.FamilyName, user.Profile.LastName),
                    new Claim("email_confirmed", user.EmailConfirmed.ToString().ToLower()),
                    new Claim("role", user.UserType.UserTypeName),
                };
                var token = _tokenService.GenerateAccessToken(authClaims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                var refreshTokenExpiryTime = DateTime.UtcNow.AddMonths(1);

                var tokenServiceQuery = new UpdateRefreshTokenCommand()
                {
                    user = _mapper.Map<UserData>(user),
                    RefreshToken = refreshToken,
                    TokenExpiryDate = refreshTokenExpiryTime
                };

                var refreshTokenData = await _mediator.Send(tokenServiceQuery);

                return Ok(new ApiResponse<TokenResponseModel>(new TokenResponseModel
                {
                    AccessToken = token.Token,
                    RefreshToken = refreshToken,
                    ValidTo = token.ValidTo,
                }));
            }

            return BadRequest(new ApiResponse("Email or Password is wrong") { Code = (int)AuthError.WrongEmailOrPassword });

        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserData>> Register([FromBody] AddUserCommand userCommand)
        {
            if (!ModelState.IsValid)
            {
                var serializableModelState = new SerializableError(ModelState);
                return BadRequest(new ApiResponse(JsonConvert.SerializeObject(serializableModelState)) { Code = (int)BaseError.ParameterError });
            }
            var result = await _mediator.Send(userCommand);

            if (!result.Success)
            {
                return BadRequest(new ApiResponse(result.Message) { Code = (int)AuthError.UserAlreadyExists });
            }

            return Ok(result);
        }

        [HttpPost("RegisterFacebook")]
        [Authorize(AuthenticationSchemes = "Facebook")]
        public async Task<IActionResult> RegisterOrLoginFacebook()
        {
            TokenResponseModel tokenResponse = null;

            var facebookUser = User;
            var firstName = facebookUser.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.GivenName).Value;
            var lastName = facebookUser.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.FamilyName).Value;
            var email = facebookUser.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value;

            var userQuery = new GetUserByEmailQuery(email);

            var userExist = await _mediator.Send(userQuery);

            if (userExist != null)
            {
                tokenResponse = await CreateTokenModel(_mapper.Map<UserData>(userExist));
                return Ok(new ApiResponse<TokenResponseModel>(tokenResponse));
            }
            var userCommand = new AddUserCommand()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = ""
            };
            var result = await _mediator.Send(userCommand);

            return Ok(result);
        }
        private async Task<TokenResponseModel> CreateTokenModel(UserData user)
        {
            var authClaims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Name, user.Profile.FirstName),
                    new Claim(JwtRegisteredClaimNames.FamilyName, user.Profile.LastName),
                    new Claim("email_confirmed", user.EmailConfirmed.ToString().ToLower()),
                    new Claim("role", user.UserType.UserTypeName),
                };

            var token = _tokenService.GenerateAccessToken(authClaims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            var refreshTokenExpiryTime = DateTime.UtcNow.AddMonths(1);
            var tokenQuery = new UpdateRefreshTokenCommand()
            {
                user = user,
                RefreshToken = refreshToken,
                TokenExpiryDate = refreshTokenExpiryTime
            };

            await _mediator.Send(tokenQuery);

            return new TokenResponseModel
            {
                AccessToken = token.Token,
                RefreshToken = refreshToken,
                ValidTo = token.ValidTo,
            };
        }
        [HttpPost("SendActivationEmail")]
        public async Task<ActionResult<ApiResponse>> SendActivationEmail([FromBody] TokenModel activationTokenCommand)
        {
            if (!ModelState.IsValid)
            {
                var serializableModelState = new SerializableError(ModelState);
                return BadRequest(new ApiResponse(JsonConvert.SerializeObject(serializableModelState)) { Code = (int)BaseError.ParameterError });
            }
            try
            {
                var user = await _mediator.Send(new GetUserByEmailQuery(activationTokenCommand.Email));

                if (user.EmailConfirmed)
                    throw new Exception();

                var activationToken = _tokenService.GenerateActivationToken();

                var confirmationEvent = await _emailService.SendActivationMail(
                    user.Email,
                    user.DisplayName,
                    activationToken,
                    $"http://localhost:3000/auth/v1/ActivateAccount?token={activationToken}"
                );

                var eventData = _mapper.Map<EmailEvent>(confirmationEvent);

                await _publishEndpoint.Publish<EmailEvent>(eventData);

                var insertActTokenCommand = new InsertActivationTokenCommand()
                {
                    Email = user.Email,
                    Token = activationToken
                };

                await _mediator.Send(insertActTokenCommand);

                return Ok(new ApiResponse(true));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message) { Code = (int)AuthError.GenericAuthError });
            }
        }

        [Route("[action]/{isShop}", Name = "GetShops")]
        [HttpGet]
        [ProducesResponseType(typeof(ActionResult<IEnumerable<ShopsInfos>>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<ShopsInfos>>> GetShops(bool isShop)
        {
            if (!ModelState.IsValid)
            {
                var serializableModelState = new SerializableError(ModelState);
                return BadRequest(new ApiResponse(JsonConvert.SerializeObject(serializableModelState)) { Code = (int)BaseError.ParameterError });
            }
            try
            {
                var response = await _mediator.Send(new GetShopsQuery(isShop));

                var finalResponse = _mapper.Map<IEnumerable<UserInfos>>(response.ToList());

                List<ShopsInfos> shopsInfos = new List<ShopsInfos>();

                foreach(var userInfo in finalResponse)
                {
                    shopsInfos.Add(new ShopsInfos
                    {
                        Id = userInfo.Id,
                        Email = userInfo.Email,
                        FirstName = userInfo.Profile.FirstName,
                        LastName = userInfo.Profile.LastName
                    });
                }

                return Ok(shopsInfos);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message) { Code = (int)AuthError.GenericAuthError });
            }
        }


        [HttpPost("ActivateAccount")]
        public async Task<ActionResult<ApiResponse>> ActivateAccount([FromBody] ActivateUserCommand model)
        {
            if (!ModelState.IsValid)
            {
                var serializableModelState = new SerializableError(ModelState);
                return BadRequest(new ApiResponse(JsonConvert.SerializeObject(serializableModelState)) { Code = (int)BaseError.ParameterError });
            }
            var token = model.Token;

            try
            {
                var user = await _mediator.Send(model);

                var tokenResponse = await CreateTokenModel(user);

                return Ok(new ApiResponse<TokenResponseModel>(tokenResponse));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message) { Code = (int)AuthError.GenericAuthError });
            }
        }


        [HttpPost("SendResetPasswordEmail")]
        public async Task<IActionResult> SendResetPasswordEmail([FromBody] TokenModel model)
        {
            if (!ModelState.IsValid)
            {
                var serializableModelState = new SerializableError(ModelState);
                return BadRequest(new ApiResponse(JsonConvert.SerializeObject(serializableModelState)) { Code = (int)BaseError.ParameterError });
            }

            var email = model.Email;

            try
            {
                var user = await _mediator.Send(new GetUserByEmailQuery(model.Email));

                if(user == null)
                    return BadRequest(new ApiResponse("User with the provided email does not exists.") { Code = (int)BaseError.ParameterError });

                var activationToken = _tokenService.GenerateActivationToken();

                var emailInformation = await _emailService.SendForgotPasswordMail(
                    user.Email,
                    user.DisplayName,
                    activationToken,
                    $"http://localhost:3000/auth/v1/ResetPassword?token={activationToken}"
                );
                //
                var eventData = _mapper.Map<EmailEvent>(emailInformation);
                //
                await _publishEndpoint.Publish<EmailEvent>(eventData);

                var insertActTokenCommand = new InsertForgotPasswordTokenCommand()
                {
                    Email = user.Email,
                    Token = activationToken
                };

                await _mediator.Send(insertActTokenCommand);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return Ok(new ApiResponse(true));
        
        }
        [HttpPost("ValidateResetPasswordToken")]
        public async Task<IActionResult> ValidatePasswordResetToken([FromBody] InsertForgotPasswordTokenCommand model)
        {
            if (!ModelState.IsValid)
            {
                var serializableModelState = new SerializableError(ModelState);
                return BadRequest(new ApiResponse(JsonConvert.SerializeObject(serializableModelState)) { Code = (int)BaseError.ParameterError });
            }

            try
            {
                var forgotPasswordToken = new GetForgotPasswordTokenQuery(model.Token)
                {
                    ForgotPasswordToken = model.Token
                };

                var response = await _mediator.Send(forgotPasswordToken);
                


                return Ok(new ApiResponse(true));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message) { Code = (int)AuthError.GenericAuthError });
            }
        }
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordParamModel model)
        {
            if (!ModelState.IsValid)
            {
                var serializableModelState = new SerializableError(ModelState);
                return BadRequest(new ApiResponse(JsonConvert.SerializeObject(serializableModelState)) { Code = (int)BaseError.ParameterError });
            }

            var token = model.Token;
            var password = model.Password;

            try
            {
                var resetToken = new ResetPasswordCommand()
                {
                    Token = model.Token,
                    Password = model.Password
                };

                var response = await _mediator.Send(resetToken);

                return Ok(new ApiResponse(true));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(ex.Message) { Code = (int)AuthError.GenericAuthError });
            }
        }
    }
}
