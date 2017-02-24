using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Models;
using Carvana.MarketExpansion.WebApi.Services;
using Carvana.MarketExpansion.WebApi.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleInjector;

namespace Carvana.MarketExpansion.WebApi.Tests
{
    [TestClass]
    public class AccountServiceTests
    {
        private Container _registry;

        [TestInitialize]
        public void Init()
        {
            RegisterTypes();
        }

        [TestMethod]
        public void Should_Register_User()
        {
            var email = "jib@jab.com";
            var userCreds = new UserCredentials {Email = email, Password = "secretPassword"};
            var accountService = _registry.GetInstance<IAccountService>();
            var tokens = accountService.Register(userCreds);

            var accountRepo = _registry.GetInstance<IAccountRepository>();
            var user = accountRepo.GetUserByEmail(email);

            Assert.AreEqual(email, user.Email);
        }

        [TestMethod]
        public void Should_Login_User()
        {
            var email = "jib2@jab.com";
            var userCreds = new UserCredentials { Email = email, Password = "secretPassword" };
            var accountService = _registry.GetInstance<IAccountService>();
            accountService.Register(userCreds);
            var tokens = accountService.Login(userCreds);

            Assert.IsNotNull(tokens.RefreshToken);
            Assert.IsNotNull(tokens.AccessToken);
        }

        [TestMethod]
        public void Should_Logout_User()
        {
            var email = "jib2@jab.com";
            var userCreds = new UserCredentials { Email = email, Password = "secretPassword" };
            var accountService = _registry.GetInstance<IAccountService>();
            var tokens = accountService.Login(userCreds);
            accountService.Logout(tokens.RefreshToken);

            var accountRepo = _registry.GetInstance<IAccountRepository>();
            var user = accountRepo.GetUserByEmail(email);

            Assert.IsNull(user.RefreshTokenId);
        }

        private void RegisterTypes()
        {
            _registry = new Container();

            _registry.Register<ISecuritySettings, SecuritySettings>(Lifestyle.Transient);
            _registry.Register<ISqlConnectionFactory, SqlConnectionFactory>(Lifestyle.Transient);
            _registry.Register<IAccountRepository, AccountRepository>(Lifestyle.Transient);
            _registry.Register<IJwtEncodingService, JwtEncodingService>(Lifestyle.Transient);
            _registry.Register<IJwtService, JwtService>(Lifestyle.Transient);
            _registry.Register<IAccessTokenService, AccessTokenService>(Lifestyle.Transient);
            _registry.Register<IRefreshTokenService, RefreshTokenService>(Lifestyle.Transient);
            _registry.Register<IPasswordService, PasswordService>(Lifestyle.Transient);
            _registry.Register<IAccountService, AccountService>(Lifestyle.Transient);
        }
    }
}
