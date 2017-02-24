using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public interface IAccountService
    {
        LoginTokens Register(UserCredentials userCredentials);
        LoginTokens Login(UserCredentials userCredentials);
        void Logout(string jwToken);
        string GetAccessToken(string refreshToken);
    }
}