using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly IPasswordService _passwordService;
        private readonly IJwtService _jwtService;

        public AccountService(
            IPasswordService passwordService, 
            IJwtService jwtService)
        {
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public string Register(UserCredentials userCredentials)
        {
            // Look up user by credentials
            // If exists:
            //        Run Login logic
            //    else:
            //        Create User and Run Login logic


            return string.Empty;
        }

        public string Login(UserCredentials userCredentials)
        {
            // Look up user by email
            //   If not exists:
            //    Return 401 Unauthorized
            //  else:
            //    Hash Password and compare against value in database
            //        If Valid:
            //          Return Access and Refresh Tokens to client
            //        else:
            //            Return 401 Unauthorized

            return string.Empty;
        }

        public void Logout(string jwToken)
        {
            // revoke refresh token
        }
    }
}