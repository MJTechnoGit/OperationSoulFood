using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace OperationSoulFood.Services.CouponAPI.Utility
{
    public class BackendApiAuthenticationHttpClientHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BackendApiAuthenticationHttpClientHandler(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // This is section is responsible for passing the access token during request. This will enable access to methods with the [Authorize] attribute.
        protected override async Task<HttpResponseMessage>  SendAsync(HttpRequestMessage request,  CancellationToken cancellationToken)
        {
            var token = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            return await base.SendAsync(request, cancellationToken);
        }

    }
}
