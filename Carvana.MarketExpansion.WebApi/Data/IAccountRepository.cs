using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Data
{
    public interface IAccountRepository
    {
        User GetUserByUserName(string userName);
        string GetUserPasswordHashByUserName(string userName);
    }
}