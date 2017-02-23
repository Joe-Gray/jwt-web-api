using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Data
{
    public interface IAccountRepository
    {
        User GetUserByEmail(string email);
        string GetUserPasswordHashByEmail(string email);
        int CreateUser(User user);
        int UpdateUserRefreshTokenId(string email, string refreshTokenId);
        int RevokeUserRefreshToken(string email);
    }
}