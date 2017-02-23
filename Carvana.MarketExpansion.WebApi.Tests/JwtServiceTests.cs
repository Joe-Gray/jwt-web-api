﻿using System.Collections.Generic;
using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Models;
using Carvana.MarketExpansion.WebApi.Services;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Carvana.MarketExpansion.WebApi.Tests
{
    [TestClass]
    public class JwtServiceTests
    {
        [TestMethod]
        public void Encode_And_Validate_JwToken()
        {
            var user = BuildUser();

            var accountRepository = A.Fake<IAccountRepository>();
            A.CallTo<User>(() => accountRepository.GetUserByUserName("gbsjoe@gmail.com"))
                .Returns(user);

            IJwtService jwtService = new JwtService(new JwtEncodingService(), accountRepository);

            var token = jwtService.CreateAccessToken("gbsjoe@gmail.com");
            var isValid = jwtService.DoesSignatureMatch(token);

            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void Encode_And_Decode_JwToken_Payload()
        {
            var user = BuildUser();

            var accountRepository = A.Fake<IAccountRepository>();
            A.CallTo<User>(() => accountRepository.GetUserByUserName("gbsjoe@gmail.com"))
                .Returns(user);

            IJwtService jwtService = new JwtService(new JwtEncodingService(), accountRepository);

            var token = jwtService.CreateAccessToken("gbsjoe@gmail.com");
            var decodedUser = jwtService.GetJwtPayload(token);

            var userJson = JsonConvert.SerializeObject(user.SecurityClaims);
            var decodedUserJson = JsonConvert.SerializeObject(decodedUser.userSecurityClaims);

            Assert.AreEqual(userJson, decodedUserJson);
        }

        private User BuildUser()
        {
            return new User
            {
                Email = "gbsjoe@gmail.com",
                Id = "123456789",
                LoginProvider = "Custom",
                ProviderKey = "987654321",
                SecurityClaims = new List<string> { "ViewMarkets", "AddMarkets" }
            };
        }
    }
}