using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SignalRSample.Api.Database;
using SignalRSample.Api.Hubs;
using SignalRSample.Api.Services;

namespace SignalRSample.Api
{
    public class Startup
    {
        private readonly string CorsPolicy = "CorsPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<UserDbContext>(
                options => options.UseInMemoryDatabase("Users"));
            services.AddSingleton<GameSessionManager>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddControllers();
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicy,
                    builder =>
                    {
                        builder
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials()
                            .WithOrigins("http://localhost:4200", "https://localhost:6001", "tictactoe://localhost");
                    });
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddJwtBearer("Bearer", options =>
                {
                    // TODO: Add to config
                    options.Authority = "https://pj-tt-idsrv.azurewebsites.net/";
                    // options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "signalr-api";
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) &&
                                (path.StartsWithSegments("/tictactoe")))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            var te = context.Exception;
                            return Task.CompletedTask;
                        },
                    };
                })
                .AddIdentityServerAuthentication("token", options =>
                {
                    // TODO: Add to config
                    options.Authority = "https://pj-tt-idsrv.azurewebsites.net/";
                    // options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "signalr-api";
                    options.ApiSecret = "secret";

                    options.JwtBearerEvents = new JwtBearerEvents
                    {
                        OnTokenValidated = e =>
                        {
                            var jwt = e.SecurityToken as JwtSecurityToken;
                            var type = jwt.Header.Typ;

                            if (!string.Equals(type, "at+jwt", StringComparison.Ordinal))
                            {
                                e.Fail("JWT is not an access token");
                            }

                            return Task.CompletedTask;
                        }
                    };
                });


            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(CorsPolicy);
            app.UseRouting();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<GamesHub>("/tictactoe");
            });
        }
    }
}