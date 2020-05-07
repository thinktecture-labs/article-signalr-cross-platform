using System;
using System.Net.Http;
using Blazorise;
using Blazorise.Icons.FontAwesome;
using Blazorise.Icons.Material;
using Blazorise.Material;
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
            services
                  .AddBlazorise(options =>
                  {
                      options.ChangeTextOnKeyPress = true; // optional
                  })
                  .AddMaterialProviders()
                  .AddMaterialIcons()
                  .AddFontAwesomeIcons();

            services.AddOidc(new Uri("http://localhost:5000"), (settings, siteUri) =>
            {
                settings.UseDefaultCallbackUris(siteUri);
                settings.ClientId = "blazor-spa";
                settings.ClientSecret = "secret";
                settings.ResponseType = "code";
                settings.Scope = "openid profile push-api.full_access";
            });
        }
    }
}
