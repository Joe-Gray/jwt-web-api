using System.Net;
using System.Web.Http;
using Carvana.MarketExpansion.WebApi.Attributes;
using Carvana.MarketExpansion.WebApi.Exceptions;
using Carvana.MarketExpansion.WebApi.Models;
using Carvana.MarketExpansion.WebApi.Services;

namespace Carvana.MarketExpansion.WebApi.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : BaseApiController
    {
        private readonly IAccountService _accountService;

        public AccountsController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [CustomAuthorization]
        [Route("logout")]
        [HttpGet]
        public IHttpActionResult Logout()
        {
            _accountService.Logout(AuthToken);
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
                var loginTokens = _accountService.Register(userCredentials);
                return Created("", loginTokens);
            }
            catch (InvalidCredentialsException)
            {
                return Content(HttpStatusCode.Unauthorized, new { error = "Invalid Credentials" });
            }
        }

        [Route("login")]
        [HttpPost]
        public IHttpActionResult Login([FromBody] UserCredentials userCredentials)
        {
            if (userCredentials == null)
            {
                return BadRequest();
            }

            try
            {
                var loginTokens = _accountService.Login(userCredentials);
                return Ok(loginTokens);
            }
            catch (InvalidCredentialsException)
            {
                return Content(HttpStatusCode.Unauthorized, new { error = "Invalid Credentials" });
            }
        }
    }
}
