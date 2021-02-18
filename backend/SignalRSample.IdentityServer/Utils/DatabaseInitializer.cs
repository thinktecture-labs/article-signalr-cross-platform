using System.Linq;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SignalRSample.IdentityServer.Data;

namespace SignalRSample.IdentityServer.Utils
{
    public static class DatabaseInitializer
    {
        public static void Initialize(IApplicationBuilder app, ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            InitializeTokenServerConfigurationDatabase(app);

            context.SaveChanges();
        }

        private static void InitializeTokenServerConfigurationDatabase(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>()
                ?.CreateScope();

            if (scope == null) return;

            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>()
                .Database.Migrate();

            scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>()
                .Database.Migrate();

            var context = scope.ServiceProvider
                .GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();
            if (!context.Clients.Any())
            {
                foreach (var client in Config.Clients)
                {
                    context.Clients.Add(client.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.IdentityResources.Any())
            {
                foreach (var resource in Config.IdentityResources)
                {
                    context.IdentityResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiResources.Any())
            {
                foreach (var resource in Config.Apis)
                {
                    context.ApiResources.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }

            if (!context.ApiScopes.Any())
            {
                foreach (var resource in Config.ApiScopes)
                {
                    context.ApiScopes.Add(resource.ToEntity());
                }

                context.SaveChanges();
            }
        }
    }
}