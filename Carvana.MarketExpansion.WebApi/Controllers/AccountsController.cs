using System.Net;
using System.Web.Http;
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

        [Route("logout")]
        [HttpGet]
        public IHttpActionResult Logout()
        {
            if (AuthToken == null)
            {
                return Content(HttpStatusCode.BadRequest, new { error = "Missing Authorization Token" });
            }

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
