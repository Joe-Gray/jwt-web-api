using System.Web.Http;
using Carvana.MarketExpansion.WebApi.Attributes;

namespace Carvana.MarketExpansion.WebApi.Controllers
{
    [RoutePrefix("api/markets")]
    public class MarketsController : BaseApiController
    {
        [AccessTokenAuthorization("ViewMarket")]
        [Route("view")]
        [HttpGet]
        public IHttpActionResult GetMarkets()
        {
            return Ok(new {message="you view market"});
        }

        [AccessTokenAuthorization("AddMarket")]
        [Route("add")]
        [HttpPost]
        public IHttpActionResult AddMarkets()
        {
            return Ok(new { message = "you add market" });
        }

        [AccessTokenAuthorization("EditMarket")]
        [Route("edit")]
        [HttpPost]
        public IHttpActionResult EditMarkets()
        {
            return Ok(new { message = "you edit market" });
        }

        [AccessTokenAuthorization("DeleteMarket")]
        [Route("delete")]
        [HttpDelete]
        public IHttpActionResult DeleteMarkets()
        {
            return Ok(new { message = "you delete market" });
        }
    }
}