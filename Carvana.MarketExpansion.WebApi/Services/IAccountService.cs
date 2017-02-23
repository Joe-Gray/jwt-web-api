using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public interface IAccountService
    {
        string Register(UserCredentials userCredentials);
        LoginTokens Login(UserCredentials userCredentials, string hashedPassword);
        void Logout(string jwToken);
    }
}