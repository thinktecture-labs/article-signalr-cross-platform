using System;
using System.Net.Http;
using BlazorSignalRSample.Client.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sotsera.Blazor.Oidc;
using Sotsera.Blazor.Oidc.Configuration.Model;
using MatBlazor;
using Microsoft.Extensions.Configuration;

namespace BlazorSignalRSample.Client
{
    public static class Startup
    {
        public static void PopulateServices(IServiceCollection services, IConfiguration configuration)
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
           
           var idSrvUrl = configuration["api:identityServerUrl"];
            services.AddOidc(new Uri($"{idSrvUrl}"), (settings, siteUri) =>
            {
                settings.UseDefaultCallbackUris(siteUri);
                settings.ClientId = "blazor-spa";
                settings.ResponseType = "code";
                settings.Scope = "openid profile signalr-api.full_access";
                settings.ClientSecret = "blazor-spa-secret";
                settings.LoadUserInfo = true;

                settings.MinimumLogeLevel = LogLevel.Debug;
                settings.StorageType = StorageType.SessionStorage;
                settings.InteractionType = InteractionType.Popup;
            });
        }
    }
}
