using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Data
{
    public interface IAccountRepository
    {
        User GetUserByEmail(string email);
        string GetUserPasswordHashByEmail(string email);
        void CreateUser(User user);
        void UpdateUserRefreshTokenId(string email, string refreshTokenId);
        void RevokeUserRefreshToken(string email);
    }
}