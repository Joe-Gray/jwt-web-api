using System.Collections.Generic;
using Carvana.MarketExpansion.WebApi.Models;
using Dapper;

namespace Carvana.MarketExpansion.WebApi.Data
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ISqlConnectionFactory _sqlConnectionFactory;

        public AccountRepository(ISqlConnectionFactory sqlConnectionFactory)
        {
            _sqlConnectionFactory = sqlConnectionFactory;
        }

        public User GetUserByUserName(string userName)
        {
            using (var conn = _sqlConnectionFactory.GetOpenSqlConnection())
            {
                var user = conn.QueryFirstOrDefault<User>("SELECT u.Id, u.Email, u.PasswordHash, l.LoginProvider, l.ProviderKey FROM dbo.AspNetUsers u JOIN dbo.AspNetUserLogins l ON l.UserId = u.Id WHERE u.Email = @Email", new {Email = userName});
                var claimValues = conn.Query<string>("SELECT c.ClaimValue FROM dbo.AspNetUsers u JOIN dbo.AspNetUserLogins l ON l.UserId = u.Id JOIN dbo.AspNetUserRoles ur ON ur.UserId = u.Id JOIN dbo.AspNetRoles r ON r.Id = ur.RoleId JOIN dbo.AspNetRoleClaims c ON c.RoleId = r.Id WHERE u.Email = @Email", new { Email = userName });
                user.SecurityClaims = new List<string>(claimValues);
                return user;
            }
        }
    }
}