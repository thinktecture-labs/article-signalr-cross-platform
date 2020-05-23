using System;
using System.Net.Http;
using BlazorSignalRSample.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Sotsera.Blazor.Oidc;

namespace BlazorSignalRSample.Client
{
    public static class Startup
    {
        public static void PopulateServices(IServiceCollection services)
        {
            services.AddTransient(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5001") });
            services.AddScoped<SignalRService>();
            services.AddScoped<UserService>();

            services.AddOidc(new Uri("http://localhost:5000"), (settings, siteUri) =>
            {
                settings.UseDefaultCallbackUris(siteUri);
                settings.ClientId = "blazor-spa";
                settings.ClientSecret = "blazor-spa-secret";
                settings.ResponseType = "code";
                settings.Scope = "openid profile signalr-api.full_access";
            });
        }
    }
}
