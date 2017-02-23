using Carvana.MarketExpansion.WebApi.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carvana.MarketExpansion.WebApi.Tests
{
    [TestClass]
    public class AccountRepositoryIntegrationTests
    {
        private string _testEmail = "test@test.com";

        //[TestMethod]
        public void Should_Get_User_By_Email()
        {
            IAccountRepository accountRepository = new AccountRepository(new SqlConnectionFactory());
            var user = accountRepository.GetUserByEmail(_testEmail);

            Assert.IsNotNull(user);
            Assert.AreEqual(_testEmail, user.Email);
        }

        //[TestMethod]
        public void Should_Get_PasswordHash_By_Email()
        {
            IAccountRepository accountRepository = new AccountRepository(new SqlConnectionFactory());
            var passwordHash = accountRepository.GetUserPasswordHashByEmail(_testEmail);

            Assert.IsNotNull(passwordHash);
        }
    }
}
