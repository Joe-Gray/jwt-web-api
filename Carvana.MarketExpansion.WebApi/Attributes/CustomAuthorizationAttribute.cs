using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Carvana.MarketExpansion.WebApi.Services;

namespace Carvana.MarketExpansion.WebApi.Attributes
{
    public class CustomAuthorizationAttribute : AuthorizationFilterAttribute
    {
        private string[] _claims;
        private readonly IJwtService _jwtService;

        public CustomAuthorizationAttribute(params string[] claims)
        {
            _claims = claims;
            _jwtService = new JwtService(new JwtEncodingService());
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            var authToken = actionContext.Request?.Headers?.Authorization?.Parameter;

            if (authToken == null)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized,
                    "Missing Authorization Token");
            }

            var isSignatureValid = _jwtService.IsSignatureValid(authToken);

            if (!isSignatureValid)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized,
                    "Authorization Token is Invalid");
            }

            var isTokenExpired = _jwtService.IsTokenExpired(authToken);

            if (isTokenExpired)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized,
                    "Authorization Token is Expired");
            }

            var isTokenRevoked = _jwtService.IsTokenRevoked(authToken);

            if (isTokenRevoked)
            {
                actionContext.Response = actionContext.Request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized,
                    "Not Authorized");
            }

            var jwtPayload = _jwtService.GetJwtPayload(authToken);

            foreach (var claim in _claims)
            {
                if (jwtPayload.userSecurityClaims.Any(c => c == claim))
                {
                    return;
                }
            }

            actionContext.Response = actionContext.Request.CreateErrorResponse(
                HttpStatusCode.Unauthorized,
                "Not Authorized");
        }
    }
}