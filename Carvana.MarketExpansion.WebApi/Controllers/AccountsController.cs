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

        [Route("logout")]
        [HttpGet]
        public IHttpActionResult Logout()
        {
            if (AuthToken != null && AuthToken != "null")
            {
                _accountService.Logout(AuthToken);
            }

            return Ok(new { message = "Logged Out!" });
        }

        [Route("register")]
        [HttpPost]
        public IHttpActionResult Register([FromBody] UserCredentials userCredentials)
        {
            if (userCredentials == null)
            {
                return Content(HttpStatusCode.BadRequest, new { errorCode = "InvalidCredentials", message = "Missing Credentials" });
            }

            try
            {
                var loginTokens = _accountService.Register(userCredentials);
                return Created("", loginTokens);
            }
            catch (InvalidCredentialsException)
            {
                return Content(HttpStatusCode.Unauthorized, new { errorCode = "InvalidCredentials", message = "Invalid Credentials" });
            }
        }

        [Route("login")]
        [HttpPost]
        public IHttpActionResult Login([FromBody] UserCredentials userCredentials)
        {
            if (userCredentials == null)
            {
                return Content(HttpStatusCode.BadRequest, new { errorCode = "InvalidCredentials", message = "Missing Credentials" });
            }

            try
            {
                var loginTokens = _accountService.Login(userCredentials);
                return Ok(loginTokens);
            }
            catch (InvalidCredentialsException)
            {
                return Content(HttpStatusCode.Unauthorized, new { errorCode = "InvalidCredentials", message = "Invalid Credentials" });
            }
        }

        [RefreshTokenAuthorization]
        [Route("getAccessToken")]
        [HttpGet]
        public IHttpActionResult GetAccessToken()
        {
            var accessToken = _accountService.GetAccessToken(Request.Headers.Authorization.Parameter);
            return Ok(new { token = accessToken });
        }
    }
}
