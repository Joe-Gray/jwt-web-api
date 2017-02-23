using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public interface IRefreshTokenService
    {
        string CreateToken(string email);
        string CreateToken(User user);
    }
}