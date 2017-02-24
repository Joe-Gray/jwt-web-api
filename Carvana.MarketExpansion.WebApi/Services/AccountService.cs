using System;
using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Exceptions;
using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly IPasswordService _passwordService;
        private readonly IAccountRepository _accountRepository;
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IJwtService _jwtService;

        public AccountService(
            IPasswordService passwordService, 
            IAccountRepository accountRepository, 
            IAccessTokenService accessTokenService, 
            IRefreshTokenService refreshTokenService, 
            IJwtService jwtService)
        {
            _passwordService = passwordService;
            _accountRepository = accountRepository;
            _accessTokenService = accessTokenService;
            _refreshTokenService = refreshTokenService;
            _jwtService = jwtService;
        }

        public LoginTokens Register(UserCredentials userCredentials)
        {
            GuardAgainstMissingCredentials(userCredentials);

            var user = _accountRepository.GetUserByEmail(userCredentials.Email);

            if (user != null)
            {
                throw new UserAlreadyRegisteredException();
            }

            var hashedPassword = _passwordService.HashPassword(userCredentials.Password);

            user = new User
            {
                Email = userCredentials.Email,
                SecurityUserGuid = Guid.NewGuid(),
                PasswordHash = hashedPassword
            };

            _accountRepository.CreateUser(user);

            var loginTokens = GetTokensForUser(user.Email);
            return loginTokens;
        }

        public LoginTokens Login(UserCredentials userCredentials)
        {
            GuardAgainstMissingCredentials(userCredentials);

            var hashedPassword = _accountRepository.GetUserPasswordHashByEmail(userCredentials.Email);

            if (string.IsNullOrWhiteSpace(hashedPassword))
            {
                throw new InvalidCredentialsException();
            }

            var isPasswordValid = _passwordService.IsPasswordValid(userCredentials.Password, hashedPassword);

            if (!isPasswordValid)
            {
                throw new InvalidCredentialsException();
            }

            var loginTokens = GetTokensForUser(userCredentials.Email);
            return loginTokens;
        }

        public void Logout(string jwToken)
        {
            var payload = _jwtService.GetJwtPayload(jwToken);
            _accountRepository.RevokeUserRefreshToken(payload.userEmail);
        }

        public string GetAccessToken(string refreshToken)
        {
            var jwtPayload = _jwtService.GetJwtPayload(refreshToken);
            var accessToken = _accessTokenService.CreateToken(jwtPayload.userEmail);
            return accessToken;
        }

        private void GuardAgainstMissingCredentials(UserCredentials userCredentials)
        {
            if (string.IsNullOrWhiteSpace(userCredentials?.Email) || string.IsNullOrWhiteSpace(userCredentials.Password))
            {
                throw new InvalidCredentialsException();
            }
        }

        private LoginTokens GetTokensForUser(string email)
        {
            var user = _accountRepository.GetUserByEmail(email);
            var accessToken = _accessTokenService.CreateToken(user);
            var refreshToken = _refreshTokenService.CreateToken(user);
            var refreshTokenPayload = _jwtService.GetJwtPayload(refreshToken);
            _accountRepository.UpdateUserRefreshTokenId(email, refreshTokenPayload.jti);

            var loginTokens = new LoginTokens(accessToken, refreshToken);
            return loginTokens;
        }
    }
}