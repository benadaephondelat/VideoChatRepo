namespace VideoChatWebApp
{
    using System;
    using System.Text;

    using Bootstrapper;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using VideoChatWebApp.Hubs;
    using VideoChatWebApp.Hubs.Interfaces;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyMethod().AllowAnyHeader().WithOrigins("https://localhost:44319");
            }));

            services.AddSignalR();

            //services.AddScoped<IHubClient, UsersHub>();

            Bootstrap.ConfigureServicesBootstrapper(services, this.Configuration);

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("CorsPolicy");

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chat");
                routes.MapHub<UsersHub>("/users");
            });

            Bootstrap.ConfigureBoostrapper(app, env);
        }
    }
}