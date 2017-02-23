using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public interface IJwtService
    {
        string CreateAccessToken(string email);
        bool DoesSignatureMatch(string jwToken);
        JwtPayload GetJwtPayload(string jwToken);
    }
}