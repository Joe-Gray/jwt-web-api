using Carvana.MarketExpansion.WebApi.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Carvana.MarketExpansion.WebApi.Tests
{
    [TestClass]
    public class PasswordServiceTests
    {
        [TestMethod]
        public void Same_Password_Should_Not_Hash_To_Same_Value()
        {
            var password = "SecretPassword@1?";
            IPasswordService passwordService = new PasswordService();

            var hashedPassword = passwordService.HashPassword(password);
            var hashedPasswordAgain = passwordService.HashPassword(password);

            Assert.AreNotEqual(hashedPassword, hashedPasswordAgain);
        }

        [TestMethod]
        public void Password_Should_Be_Equal_To_Hash()
        {
            var password = "SecretPassword@1?";
            IPasswordService passwordService = new PasswordService();

            var hashedPassword = passwordService.HashPassword(password);
            var areEqual = passwordService.IsPasswordValid(password, hashedPassword);

            Assert.IsTrue(areEqual);
        }
    }
}
