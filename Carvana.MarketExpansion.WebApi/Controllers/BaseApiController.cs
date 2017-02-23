using System.Web.Http;

namespace Carvana.MarketExpansion.WebApi.Controllers
{
    public class BaseApiController : ApiController
    {
        private string _authToken;

        protected string AuthToken => _authToken ?? (_authToken = GetAuthTokenFromHeader());

        private string GetAuthTokenFromHeader()
        {
            return Request?.Headers?.Authorization?.Parameter;
        }
    }
}