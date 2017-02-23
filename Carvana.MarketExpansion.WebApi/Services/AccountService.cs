using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Exceptions;
using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AccountService(
            IPasswordService passwordService, 
            IJwtService jwtService, 
            IAccountRepository accountRepository, 
            IAccessTokenService accessTokenService, 
            IRefreshTokenService refreshTokenService)
        {
            _passwordService = passwordService;
            _jwtService = jwtService;
            _accountRepository = accountRepository;
            _accessTokenService = accessTokenService;
            _refreshTokenService = refreshTokenService;
        }

        public string Register(UserCredentials userCredentials)
        {
            // Look up user by credentials
            // If exists:
            //        Run Login logic
            //    else:
            //        Create User and Run Login logic


            return string.Empty;
        }

        public LoginTokens Login(UserCredentials userCredentials, string hashedPassword)
        {
            var isPasswordValid = _passwordService.IsPasswordValid(userCredentials.Password, hashedPassword);

            if (!isPasswordValid)
            {
                throw new InvalidCredentialsException();
            }

            var user = _accountRepository.GetUserByUserName(userCredentials.Email);
            var accessToken = _accessTokenService.CreateToken(user);
            var refreshToken = _refreshTokenService.CreateToken(user);

            var loginTokens = new LoginTokens(accessToken, refreshToken);
            return loginTokens;
        }

        public void Logout(string jwToken)
        {
            // revoke refresh token
        }
    }
}