using System.Web.Http;
using Carvana.MarketExpansion.WebApi.Attributes;

namespace Carvana.MarketExpansion.WebApi.Controllers
{
    public class MarketsController : BaseApiController
    {
        [CustomAuthorization("ViewMarket")]
        [Route("view")]
        [HttpGet]
        public IHttpActionResult GetMarkets()
        {
            return Ok();
        }

        [CustomAuthorization("AddMarket")]
        [Route("add")]
        [HttpGet]
        public IHttpActionResult AddMarkets()
        {
            return Ok();
        }

        [CustomAuthorization("EditMarket")]
        [Route("edit")]
        [HttpGet]
        public IHttpActionResult EditMarkets()
        {
            return Ok();
        }

        [CustomAuthorization("DeleteMarket")]
        [Route("delete")]
        [HttpGet]
        public IHttpActionResult DeleteMarkets()
        {
            return Ok();
        }
    }
}