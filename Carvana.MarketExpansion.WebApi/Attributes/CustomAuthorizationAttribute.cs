using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Carvana.MarketExpansion.WebApi.Data;
using Carvana.MarketExpansion.WebApi.Services;

namespace Carvana.MarketExpansion.WebApi.Attributes
{
    public class CustomAuthorizationAttribute : AuthorizationFilterAttribute
    {
        private readonly string[] _claims;
        private readonly IJwtService _jwtService;

        public CustomAuthorizationAttribute(params string[] claims)
        {
            _claims = claims;
            _jwtService = new JwtService(new JwtEncodingService(), new AccountRepository(new SqlConnectionFactory()));
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authToken = actionContext.Request?.Headers?.Authorization?.Parameter;

            if (authToken == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new { errorCode = "MissingToken", errorMessage = "Missing Token" });
            }

            var isSignatureValid = _jwtService.IsSignatureValid(authToken);

            if (!isSignatureValid)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new { errorCode = "InvalidToken", errorMessage = "Invalid Token" });
            }

            var isTokenExpired = _jwtService.IsTokenExpired(authToken);

            if (isTokenExpired)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new { errorCode = "TokenExpired", errorMessage = "Token Expired" });
            }

            var isTokenRevoked = _jwtService.IsTokenRevoked(authToken);

            if (isTokenRevoked)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new {errorCode = "TokenRevoked", errorMessage = "Token Revoked"});

            }

            var isAnyClaimInToken = _jwtService.IsAnyClaimInToken(authToken, _claims);

            if (!isAnyClaimInToken)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new { errorCode = "MissingClaim", errorMessage = "Missing Claim" });
            }
        }
    }
}