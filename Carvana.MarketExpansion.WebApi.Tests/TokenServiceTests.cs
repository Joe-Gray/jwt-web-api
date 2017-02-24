using System;
using System.Collections.Generic;
using System.Threading;
using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Models;
using Carvana.MarketExpansion.WebApi.Services;
using Carvana.MarketExpansion.WebApi.Settings;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Carvana.MarketExpansion.WebApi.Tests
{
    [TestClass]
    public class TokenServiceTests
    {
        private IAccountRepository _accountRepository;
        private User _user;
        private IJwtService _jwtService;
        private IAccessTokenService _accessTokenService;
        private IRefreshTokenService _refreshTokenService;

        [TestInitialize]
        public void Init()
        {
            _user = new User
            {
                Email = "gbsjoe@gmail.com",
                SecurityUserGuid = Guid.NewGuid(),
                RefreshTokenId = "000123456789",
                SecurityClaims = new List<string> { "ViewMarkets", "AddMarkets" }
            };

            _accountRepository = A.Fake<IAccountRepository>();
            A.CallTo(() => _accountRepository.GetUserByEmail(A<string>.Ignored))
                .Returns(_user);

            _jwtService = new JwtService(new JwtEncodingService(), _accountRepository);

            _accessTokenService = new AccessTokenService(_jwtService, _accountRepository, new SecuritySettings());
            _refreshTokenService = new RefreshTokenService(_jwtService, _accountRepository, new SecuritySettings());
        }

        [TestMethod]
        public void Encode_And_Validate_JwToken()
        {
            var accessToken = _accessTokenService.CreateToken("gbsjoe@gmail.com");

            var isValid = _jwtService.IsSignatureValid(accessToken);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Encode_And_Decode_JwToken_Payload()
        {
            var accessToken = _accessTokenService.CreateToken("gbsjoe@gmail.com");

            var decodedUser = _jwtService.GetJwtPayload(accessToken);

            var userJson = JsonConvert.SerializeObject(_user.SecurityClaims);
            var decodedUserJson = JsonConvert.SerializeObject(decodedUser.userSecurityClaims);

            Assert.AreEqual(userJson, decodedUserJson);
        }

        [TestMethod]
        public void JwToken_Should_Be_Expired()
        {
            var securitySettings = A.Fake<ISecuritySettings>();
            A.CallTo(() => securitySettings.AccessTokenLifespanSeconds).Returns(1);

            IAccessTokenService accessTokenService = new AccessTokenService(_jwtService, _accountRepository, securitySettings);

            var accessToken = accessTokenService.CreateToken("fake");

            // set to expire in 1 second
            Thread.Sleep(2000);

            var isTokenExpired = _jwtService.IsTokenExpired(accessToken);

            Assert.IsTrue(isTokenExpired);
        }

        [TestMethod]
        public void JwToken_Should_Not_Be_Expired()
        {
            var accessToken = _accessTokenService.CreateToken("fake");

            var isTokenExpired = _jwtService.IsTokenExpired(accessToken);

            Assert.IsFalse(isTokenExpired);
        }

        [TestMethod]
        public void JwToken_Should_Be_Revoked()
        {
            _user.RefreshTokenId = null;
            var refreshToken = _refreshTokenService.CreateToken("fake");

            var isTokenRevoked = _jwtService.IsTokenRevoked(refreshToken);

            Assert.IsTrue(isTokenRevoked);
        }

        [TestMethod]
        public void JwToken_Should_Not_Be_Revoked()
        {
            var refreshToken = _refreshTokenService.CreateToken("fake");

            var isTokenRevoked = _jwtService.IsTokenRevoked(refreshToken);

            Assert.IsFalse(isTokenRevoked);
        }

        [TestMethod]
        public void JwToken_Should_Have_Valid_Claim()
        {
            var accessToken = _accessTokenService.CreateToken("fake");

            var isAnyClaimInToken = _jwtService.IsAnyClaimInToken(accessToken, new[] { "AddMarkets" });

            Assert.IsTrue(isAnyClaimInToken);
        }

        [TestMethod]
        public void JwToken_Should_Not_Have_Valid_Claim()
        {
            var accessToken = _accessTokenService.CreateToken("fake");

            var isAnyClaimInToken = _jwtService.IsAnyClaimInToken(accessToken, new[] {"DeleteMarkets"});

            Assert.IsFalse(isAnyClaimInToken);
        }
    }
}
