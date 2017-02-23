using System.Net;
using System.Web.Http;
using Carvana.MarketExpansion.WebApi.Models;
using Carvana.MarketExpansion.WebApi.Services;

namespace Carvana.MarketExpansion.WebApi.Controllers
{
    [RoutePrefix("api/accounts")]
    public class AccountsController : ApiController
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

            //_locationRepository.Insert(location);

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

            var loginFailed = true;

            if (loginFailed)
            {
                return Content(HttpStatusCode.Forbidden, new {error = "Invalid Login Credentials"});
            }

            return Ok();
        }
    }
}
