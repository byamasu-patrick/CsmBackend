using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User.Application.Models;

namespace User.Application.Contracts.Infrastructure
{
    public interface ITokenService
    {
        TokenResponse GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        void ValidateAccessToken(string accessToken);
        string GenerateActivationToken();
    }
}
