using System;
using System.Net;
using System.Web.Http;
using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Exceptions;
using Carvana.MarketExpansion.WebApi.Models;
using Carvana.MarketExpansion.WebApi.Services;

namespace Carvana.MarketExpansion.WebApi.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : ApiController
    {
        private readonly IAccountService _accountService;
        private readonly IAccountRepository _accountRepository;

        public AccountsController(
            IAccountService accountService, 
            IAccountRepository accountRepository)
        {
            _accountService = accountService;
            _accountRepository = accountRepository;
        }

        [Route("logout")]
        [HttpGet]
        public IHttpActionResult Logout()
        {
            // get JWT from header and revoke it

            return Ok(new { Message = "Logged Out!" });
        }

        [Route("register")]
        [HttpPost]
        public IHttpActionResult Register([FromBody] UserCredentials userCredentials)
        {
            if (userCredentials == null)
            {
                return BadRequest();
            }

            try
            {
                _accountService.Register(userCredentials);
            }
            catch (InvalidCredentialsException)
            {
                return Content(HttpStatusCode.Unauthorized, new { error = "Invalid Credentials" });
            }


            return Created("", new {});
        }

        [Route("login")]
        [HttpPost]
        public IHttpActionResult Login([FromBody] UserCredentials userCredentials)
        {
            if (userCredentials == null)
            {
                return BadRequest();
            }

            var passwordHash = _accountRepository.GetUserPasswordHashByEmail(userCredentials.Email);

            if (string.IsNullOrWhiteSpace(passwordHash))
            {
                return Content(HttpStatusCode.Unauthorized, new { error = "Invalid Credentials" });
            }

            try
            {
                var loginTokens = _accountService.Login(userCredentials, passwordHash);
                return Ok(loginTokens);
            }
            catch (InvalidCredentialsException)
            {
                return Content(HttpStatusCode.Unauthorized, new { error = "Invalid Credentials" });
            }
        }
    }
}
