using System.Web.Http;
using Carvana.MarketExpansion.WebApi.Attributes;

namespace Carvana.MarketExpansion.WebApi.Controllers
{
    public class MarketsController : BaseApiController
    {
        [AccessTokenAuthorization("ViewMarket")]
        [Route("view")]
        [HttpGet]
        public IHttpActionResult GetMarkets()
        {
            return Ok();
        }

        [AccessTokenAuthorization("AddMarket")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult AddMarkets()
        {
            return Ok();
        }

        [AccessTokenAuthorization("EditMarket")]
        [Route("edit")]
        [HttpGet]
        public IHttpActionResult EditMarkets()
        {
            return Ok();
        }

        [AccessTokenAuthorization("DeleteMarket")]
        [Route("delete")]
        [HttpGet]
        public IHttpActionResult DeleteMarkets()
        {
            return Ok();
        }
    }
}