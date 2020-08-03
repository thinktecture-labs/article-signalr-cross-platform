using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorSignalRSample.Client.Services
{
    public class CustomAuthorizationMessageHandler : AuthorizationMessageHandler
    {
        public CustomAuthorizationMessageHandler(IAccessTokenProvider provider,
            NavigationManager navigationManager)
            : base(provider, navigationManager)
        {
            ConfigureHandler(
               authorizedUrls: new[] { "https://localhost:5002" });
            Provider = provider;
        }

        public IAccessTokenProvider Provider { get; }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) 
        {
            var accessTokenState = await Provider.RequestAccessToken();
            if (accessTokenState.TryGetToken(out var accessToken)) {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken.Value);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}