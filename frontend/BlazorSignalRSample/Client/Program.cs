using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Blazorise.Material;
using Blazorise.Icons.Material;

namespace BlazorSignalRSample.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            // builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            Startup.PopulateServices(builder.Services);

            var host = builder.Build();
            host.Services
                  .UseMaterialProviders()
                  .UseMaterialIcons();

            await host.RunAsync();
        }
    }
}
