using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Data
{
    public interface IAccountRepository
    {
        User GetUserByEmail(string email);
        string GetUserPasswordHashByEmail(string email);
    }
}