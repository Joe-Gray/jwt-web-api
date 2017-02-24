using System;
using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Models;
using Carvana.MarketExpansion.WebApi.Settings;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly ISecuritySettings _securitySettings;
        private readonly IJwtService _jwtService;
        private readonly IAccountRepository _accountRepository;

        public AccessTokenService(
            IJwtService jwtService, 
            IAccountRepository accountRepository, 
            ISecuritySettings securitySettings)
        {
            _jwtService = jwtService;
            _accountRepository = accountRepository;
            _securitySettings = securitySettings;
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
                (int)
                currentUtcTime.AddSeconds(_securitySettings.AccessTokenLifespanSeconds)
                    .Subtract(_jwtService.EpochTime)
                    .TotalSeconds;

            return expirationInSeconds;
        }
    }
}