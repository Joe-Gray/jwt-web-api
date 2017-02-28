using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using Carvana.MarketExpansion.WebApi.Models;

namespace Carvana.MarketExpansion.WebApi.Attributes
{
    public class RefreshTokenAuthorizationAttribute : BaseTokenAuthorizationAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            if (actionContext.Response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return;
            }

            var jwtPayload = JwtService.GetJwtPayload(AuthToken);

            if (jwtPayload.tokenType != JwTokenType.Refresh.ToString())
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new { errorCode = "TokenNotRefreshType", message = "Token is not a Refresh Token" });

                return;

            }
        }
    }
}