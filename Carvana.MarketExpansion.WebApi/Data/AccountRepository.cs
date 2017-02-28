using System;
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
            using (var conn = _sqlConnectionFactory.GetOpenAuthDatabaseSqlConnection())
            {
                var user = conn.QueryFirstOrDefault<User>("SELECT SecurityUserId, SecurityUserGuid, RefreshTokenId, Email, PasswordHash FROM MarketAuth.tblSecurityUser WHERE Email = @Email", new {Email = email});
                if (user == null)
                {
                    return null;
                }
                var claimValues = conn.Query<string>("SELECT DISTINCT c.Name FROM MarketAuth.tblSecurityUser u JOIN MarketAuth.tblSecurityRoleUser ur ON ur.SecurityUserId = u.SecurityUserId JOIN MarketAuth.tblSecurityRole r ON r.SecurityRoleId = ur.SecurityRoleId JOIN MarketAuth.tblSecurityRoleClaim rc ON rc.SecurityRoleId = r.SecurityRoleId JOIN MarketAuth.tblSecurityClaim c ON c.SecurityClaimId = rc.SecurityClaimId WHERE u.Email = @Email", new { Email = email });
                user.SecurityClaims = new List<string>(claimValues);
                return user;
            }
        }

        public string GetUserPasswordHashByEmail(string email)
        {
            using (var conn = _sqlConnectionFactory.GetOpenAuthDatabaseSqlConnection())
            {
                var passwordHash = conn.QueryFirstOrDefault<string>("SELECT PasswordHash FROM MarketAuth.tblSecurityUser WHERE Email = @Email", new { Email = email });
                return passwordHash;
            }
        }

        public int CreateUser(User user)
        {
            using (var conn = _sqlConnectionFactory.GetOpenAuthDatabaseSqlConnection())
            {
                var rowsAffected =
                    conn.Execute(
                        "INSERT INTO MarketAuth.tblSecurityUser (SecurityUserGuid, Email, PasswordHash, RefreshTokenId, RowLoadedDateTime, RowUpdatedDateTime) VALUES (@SecurityUserGuid, @Email, @PasswordHash, @RefreshTokenId, @now, @now)",
                        new {user.SecurityUserGuid, user.Email, user.PasswordHash, user.RefreshTokenId, now = DateTime.UtcNow});

                return rowsAffected;
            }
        }

        public int UpdateUserRefreshTokenId(string email, string refreshTokenId)
        {
            using (var conn = _sqlConnectionFactory.GetOpenAuthDatabaseSqlConnection())
            {
                var rowsAffected =
                    conn.Execute("UPDATE MarketAuth.tblSecurityUser SET RefreshTokenId = @RefreshTokenId WHERE Email = @Email",
                        new {Email = email, RefreshTokenId = refreshTokenId});

                return rowsAffected;
            }
        }

        public int RevokeUserRefreshToken(string email)
        {
            using (var conn = _sqlConnectionFactory.GetOpenAuthDatabaseSqlConnection())
            {
                var rowsAffected =
                    conn.Execute("UPDATE MarketAuth.tblSecurityUser SET RefreshTokenId = NULL WHERE Email = @Email",
                        new { Email = email });

                return rowsAffected;
            }
        }
    }
}