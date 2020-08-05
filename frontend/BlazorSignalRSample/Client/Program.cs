using System;
using System.Net.Http;
using System.Threading.Tasks;
using MatBlazor;
using BlazorSignalRSample.Client.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace BlazorSignalRSample.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddScoped<SignalRService>();
            builder.Services
                .AddHttpClient("Blazor.ServerAPI", client => client.BaseAddress = new Uri("http://localhost:5002"))
                .AddHttpMessageHandler(sp => 
                {
                    var handler = sp.GetService<AuthorizationMessageHandler>()
                        .ConfigureHandler(
                            authorizedUrls: new[] { "http://localhost:5002" });
                    return handler;
                });
            builder.Services
                    .AddScoped(services => services.GetRequiredService<IHttpClientFactory>()
                    .CreateClient("Blazor.ServerAPI"));


            builder.Services.AddMatToaster(config =>
            {
                config.Position = MatToastPosition.TopRight;
                config.PreventDuplicates = true;
                config.NewestOnTop = true;
                config.ShowCloseButton = true;
                config.MaximumOpacity = 95;
                config.VisibleStateDuration = 3000;
            });

            builder.Services.AddOidcAuthentication(options =>
             {
                 builder.Configuration.Bind("Oidc",  options.ProviderOptions);
                 options.UserOptions.RoleClaim = "role";
             });
            builder.Services.AddApiAuthorization();
            await builder.Build().RunAsync();
        }
    }
}
