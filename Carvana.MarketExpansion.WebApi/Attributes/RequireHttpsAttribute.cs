using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using System.Web.Http.Results;

namespace Carvana.MarketExpansion.WebApi.Attributes
{
    public class RequireHttpsAttribute : IAuthenticationFilter
    {
        public bool AllowMultiple
        {
            get { return true; }
        }

        public Task AuthenticateAsync(
            HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            if (context.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
            {
                context.ErrorResult = new StatusCodeResult(HttpStatusCode.Forbidden, context.Request);
            }
            return Task.FromResult<object>(null);
        }

        public Task ChallengeAsync(
            HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            return Task.FromResult<object>(null);
        }
    }
}