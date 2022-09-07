using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using User.Application.Contracts.Persistence;
using User.Domain.Common;
using User.Domain.Entities;
using User.Infrastructure.Persistence;
using Npgsql;
using Microsoft.EntityFrameworkCore;
using User.Application.Enums;
using User.Application.Exceptions;

namespace User.Infrastructure.Repositories
{
    public class UserRepository : RepositoryBase<UserData>, IUserRepository
    {
        public UserRepository(UserContext dbContext) : base(dbContext)
        {
        }


        public async Task<UserData> ActivateUser(ActivationToken activationToken)
        {
            var user = await _dbContext.UserData
                .Include(u => u.UserType)
                .Include(u => u.Profile)
                .FirstOrDefaultAsync(u => u.Email == activationToken.Email);

            var activationInfo = await _dbContext.ActivationTokens.
                FirstOrDefaultAsync(t => t.Token == activationToken.Token && t.Email == activationToken.Email);
            
            user.EmailConfirmed = true;
            activationToken.IsProcessed = true;

            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task<ActivationToken> GetActivationToken(string activationToken)
        {
            var token = await _dbContext.ActivationTokens.FirstOrDefaultAsync(a => a.Token == activationToken);


            return token;
        }
        public async Task<ApplicationToken> ValidateApplicationToken(string token, TokenType tokenType)
        {
            return await GetApplicationToken(token, tokenType);
        }

        private async Task<ApplicationToken> GetApplicationToken(string token, TokenType tokenType)
        {

            ApplicationToken tokenModel = tokenType switch
            {
                TokenType.Activation => await GetActivationToken(token),
                TokenType.ResetPassword => await GetForgotPasswordToken(token),
                _ => throw new ArgumentOutOfRangeException(nameof(tokenType), $"Not expected tokenType value: {tokenType}")
            };

            if (tokenModel is null)
            {
                throw new ActivationTokenNotFoundException();
            }
            if (DateTime.UtcNow > tokenModel.ExpiryDate)
            {
                throw new ActivationTokenExpiredException();
            }
            if (tokenModel.IsProcessed)
            {
                throw new ActivationTokenProcessedException();
            }

            return tokenModel;
        }


        public async Task<ForgotPasswordToken> GetForgotPasswordToken(string forgotPasswordToken)
        {
            var token = await _dbContext.ForgotPasswordTokens.FirstOrDefaultAsync(a => a.Token == forgotPasswordToken);
            
            return token;
        }

        public async Task<UserData> GetUserByEmail(string email)
        {
            var user = await _dbContext.UserData
                .Include(u => u.UserType)
                .Include(u => u.Profile)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);

            return user;
        }

        public async Task InsertActivationToken(ActivationToken activationToken)
        {
            activationToken.CreatedAt = DateTime.UtcNow;
            activationToken.ExpiryDate = DateTime.UtcNow.AddMinutes(15);
            activationToken.IsProcessed = false;

            await _dbContext.ActivationTokens.AddAsync(activationToken);

            await _dbContext.SaveChangesAsync();
        }

        public async Task InsertForgotPasswordToken(ForgotPasswordToken forgotPasswordToken)
        {

            forgotPasswordToken.CreatedAt = DateTime.UtcNow;
            forgotPasswordToken.ExpiryDate = DateTime.UtcNow.AddMinutes(15);
            forgotPasswordToken.IsProcessed = false;

            await _dbContext.ForgotPasswordTokens.AddAsync(forgotPasswordToken);

            await _dbContext.SaveChangesAsync();
        }

        public async Task ResetPassword(string email, string hash, string salt)
        {
            var user = await _dbContext.UserData.FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
            user.Password = hash;
            user.Salt = salt;

            await _dbContext.SaveChangesAsync();
        }

        public void UpdateRefreshToken(UserData user, string refreshToken, DateTime tokenExpiryDate)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = tokenExpiryDate;

            _dbContext.UserData.Update(user);

            _dbContext.SaveChangesAsync();
        }

    }
}
