using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public interface IAccountService
    {
        string Register(UserCredentials userCredentials);
        string Login(UserCredentials userCredentials);
        void Logout(string jwToken);
    }
}