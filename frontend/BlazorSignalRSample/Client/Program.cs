using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorSignalRSample.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");
            
            Startup.PopulateServices(builder.Services, builder.Configuration);
            var host = builder.Build();
            await host.RunAsync();
        }
    }
}
