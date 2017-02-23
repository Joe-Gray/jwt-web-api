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

        public User GetUserByEmail(string email)
        {
            using (var conn = _sqlConnectionFactory.GetOpenSqlConnection())
            {
                var user = conn.QueryFirstOrDefault<User>("SELECT Id, Email, PasswordHash FROM dbo.MarketSecurityUser WHERE Email = @Email", new {Email = email});
                if (user == null)
                {
                    return null;
                }
                var claimValues = conn.Query<string>("SELECT c.Name FROM dbo.MarketSecurityUser u JOIN dbo.MarketSecurityUserRole ur ON ur.MarketSecurityUserId = u.Id JOIN dbo.MarketSecurityRole r ON r.Id = ur.MarketSecurityRoleId JOIN dbo.MarketSecurityClaim c ON c.MarketSecurityRoleId = r.Id WHERE u.Email = @Email", new { Email = email });
                user.SecurityClaims = new List<string>(claimValues);
                return user;
            }
        }

        public string GetUserPasswordHashByEmail(string email)
        {
            using (var conn = _sqlConnectionFactory.GetOpenSqlConnection())
            {
                var passwordHash = conn.QueryFirstOrDefault<string>("SELECT PasswordHash FROM dbo.MarketSecurityUser WHERE Email = @Email", new { Email = email });
                return passwordHash;
            }
        }

        public int CreateUser(User user)
        {
            using (var conn = _sqlConnectionFactory.GetOpenSqlConnection())
            {
                var rowsAffected =
                    conn.Execute(
                        "INSERT INTO MarketSecurityUser (Id, Email, PasswordHash, RefreshTokenId) VALUES (@Id, @Email, @PasswordHash, @RefreshTokenId)",
                        new {user.Id, user.Email, user.PasswordHash, user.RefreshTokenId});

                return rowsAffected;
            }
        }

        public int UpdateUserRefreshTokenId(string email, string refreshTokenId)
        {
            using (var conn = _sqlConnectionFactory.GetOpenSqlConnection())
            {
                var rowsAffected =
                    conn.Execute("UPDATE MarketSecurityUser SET RefreshTokenId = @RefreshTokenId WHERE Email = @Email",
                        new {Email = email, RefreshTokenId = refreshTokenId});

                return rowsAffected;
            }
        }

        public int RevokeUserRefreshToken(string email)
        {
            using (var conn = _sqlConnectionFactory.GetOpenSqlConnection())
            {
                var rowsAffected =
                    conn.Execute("UPDATE MarketSecurityUser SET RefreshTokenId = NULL WHERE Email = @Email",
                        new { Email = email });

                return rowsAffected;
            }
        }
    }
}