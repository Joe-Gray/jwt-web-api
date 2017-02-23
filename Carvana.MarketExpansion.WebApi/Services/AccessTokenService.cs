using System;
using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly int _accessTokenLifespanMinutes = 20;
        private readonly IJwtService _jwtService;
        private readonly IAccountRepository _accountRepository;

        public AccessTokenService(
            IJwtService jwtService, 
            IAccountRepository accountRepository)
        {
            _jwtService = jwtService;
            _accountRepository = accountRepository;
        }

        public string CreateToken(string email)
        {
            var user = _accountRepository.GetUserByEmail(email);
            return CreateToken(user);
        }

        public string CreateToken(User user)
        {
            var jwtPayload = CreateJwtPayload(user);
            var token = _jwtService.CreateToken(jwtPayload);
            return token;
        }

        private JwtPayload CreateJwtPayload(User user)
        {
            var issuedAt = DateTime.UtcNow;
            var tokenExpirationInSeconds = GetAccessTokenExpirationInSeconds(issuedAt);
            var jwtPayload = _jwtService.CreateJwtPayload(user, issuedAt, tokenExpirationInSeconds, "access");
            jwtPayload.userSecurityClaims = user.SecurityClaims;
            return jwtPayload;
        }

        private int GetAccessTokenExpirationInSeconds(DateTime currentUtcTime)
        {
            var expirationInSeconds =
                (int)currentUtcTime.AddMinutes(_accessTokenLifespanMinutes).Subtract(_jwtService.EpochTime).TotalSeconds;

            return expirationInSeconds;
        }
    }
}