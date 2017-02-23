using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public interface IAccessTokenService
    {
        string CreateToken(string email);
        string CreateToken(User user);
    }
}