using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Services;

namespace Carvana.MarketExpansion.WebApi.Attributes
{
    public class BaseTokenAuthorizationAttribute : AuthorizationFilterAttribute
    {
        protected readonly IJwtService JwtService;
        protected string AuthToken;

        public BaseTokenAuthorizationAttribute()
        {
            JwtService = new JwtService(new JwtEncodingService(), new AccountRepository(new SqlConnectionFactory()));
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            AuthToken = actionContext.Request?.Headers?.Authorization?.Parameter;

            if (AuthToken == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new { errorCode = "MissingToken", message = "Missing Token" });

                return;
            }

            var isSignatureValid = JwtService.IsSignatureValid(AuthToken);

            if (!isSignatureValid)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new { errorCode = "InvalidToken", message = "Invalid Token" });

                return;
            }

            var isTokenExpired = JwtService.IsTokenExpired(AuthToken);

            if (isTokenExpired)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new { errorCode = "TokenExpired", message = "Token Expired" });

                return;
            }

            var isTokenRevoked = JwtService.IsTokenRevoked(AuthToken);

            if (isTokenRevoked)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new { errorCode = "TokenRevoked", message = "Token Revoked" });

                return;

            }
        }
    }
}