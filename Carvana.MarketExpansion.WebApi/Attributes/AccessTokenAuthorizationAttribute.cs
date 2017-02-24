using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;

namespace Carvana.MarketExpansion.WebApi.Attributes
{
    public class AccessTokenAuthorizationAttribute : BaseTokenAuthorizationAttribute
    {
        private readonly string[] _claims;

        public AccessTokenAuthorizationAttribute(params string[] claims) 
            : base()
        {
            _claims = claims ?? new string[]{};
        }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);

            if (actionContext.Response.StatusCode == HttpStatusCode.Unauthorized)
            {
                return;
            }

            var isAnyClaimInToken = JwtService.IsAnyClaimInToken(AuthToken, _claims);

            if (!isAnyClaimInToken)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                    new { errorCode = "MissingClaim", errorMessage = "Missing Claim" });

                return;
            }
        }
    }
}