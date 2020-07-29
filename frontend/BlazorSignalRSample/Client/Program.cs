using System.Threading.Tasks;
using MatBlazor;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using BlazorSignalRSample.Client.Services;

namespace BlazorSignalRSample.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            builder.Services.AddTransient<HttpClient>();
            builder.Services.AddScoped<SignalRService>();

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
                 builder.Configuration.Bind("Oidc", options.ProviderOptions);
             });

            
            builder.Services.AddApiAuthorization();
            
            await builder.Build().RunAsync();
        }
    }
}
