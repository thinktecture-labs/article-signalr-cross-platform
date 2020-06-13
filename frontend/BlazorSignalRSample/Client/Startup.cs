using System;
using System.Net.Http;
using BlazorSignalRSample.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sotsera.Blazor.Oidc;
using Sotsera.Blazor.Oidc.Configuration.Model;
using MatBlazor;

namespace BlazorSignalRSample.Client
{
    public static class Startup
    {
        public static void PopulateServices(IServiceCollection services)
        {
            services.AddTransient<HttpClient>();
            services.AddScoped<SignalRService>();

            services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.TopRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });
           
            services.AddOidc(new Uri("https://pj-tt-idsrv.azurewebsites.net"), (settings, siteUri) =>
            {
                settings.UseDefaultCallbackUris(siteUri);
                settings.ClientId = "blazor-spa";
                settings.ResponseType = "code";
                settings.Scope = "openid profile signalr-api.full_access";
                settings.ClientSecret = "blazor-spa-secret";

                settings.MinimumLogeLevel = LogLevel.Information;
                settings.StorageType = StorageType.SessionStorage;
                settings.InteractionType = InteractionType.Popup;
            });
        }
    }
}
