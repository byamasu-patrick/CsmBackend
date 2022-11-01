using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Enums;
using User.Domain.Entities;

namespace User.Application.Contracts.Persistence
{
    public interface IUserRepository : IAsyncRepository<UserData>
    {
        Task<UserData> GetUserByEmail(string email);

        Task<IEnumerable<UserData>> GetShops(bool shops);
        Task<ActivationToken> GetActivationToken(string activationToken);
        Task InsertActivationToken(ActivationToken activationToken);
        void UpdateRefreshToken(UserData user, string refreshToken, DateTime tokenExpiryDate);
        Task<UserData> ActivateUser(ActivationToken activationToken);
        Task<ForgotPasswordToken> GetForgotPasswordToken(string forgotPasswordToken);
        Task InsertForgotPasswordToken(ForgotPasswordToken forgotPasswordToken);
        Task ResetPassword(string email, string hash, string salt);
        Task<ApplicationToken> ValidateApplicationToken(string token, TokenType tokenType);
    }
}
