using Carvana.MarketExpansion.WebApi.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carvana.MarketExpansion.WebApi.Tests
{
    [TestClass]
    public class AccountRepositoryIntegrationTests
    {
        //[TestMethod]
        public void Should_Get_User_By_Email()
        {
            IAccountRepository accountRepository = new AccountRepository(new SqlConnectionFactory());
            var user = accountRepository.GetUserByUserName("gbsjoe@gmail.com");

            Assert.IsNotNull(user);
            Assert.AreEqual("gbsjoe@gmail.com", user.Email);
        }

        //[TestMethod]
        public void Should_Get_PasswordHash_By_Email()
        {
            IAccountRepository accountRepository = new AccountRepository(new SqlConnectionFactory());
            var passwordHash = accountRepository.GetUserPasswordHashByUserName("gbsjoe@gmail.com");

            Assert.IsNotNull(passwordHash);
        }
    }
}
