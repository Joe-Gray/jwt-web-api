using System;
using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly int _refreshTokenLifespanDays = 14;
        private readonly IJwtService _jwtService;
        private readonly IAccountRepository _accountRepository;

        public RefreshTokenService(
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
            var tokenExpirationInSeconds = GetRefreshTokenExpirationInSeconds(issuedAt);
            var jwtPayload = _jwtService.CreateJwtPayload(user, issuedAt, tokenExpirationInSeconds, "refresh");
            return jwtPayload;
        }

        private int GetRefreshTokenExpirationInSeconds(DateTime currentUtcTime)
        {
            var expirationInSeconds =
                (int)currentUtcTime.AddDays(_refreshTokenLifespanDays).Subtract(_jwtService.EpochTime).TotalSeconds;

            return expirationInSeconds;
        }
    }
}