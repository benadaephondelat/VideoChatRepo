namespace Bootstrapper
{
    using System;
    using System.Text;

    using DAL;
    using Models;

    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.SpaServices.AngularCli;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.IdentityModel.Tokens;
    using ServiceLayer.Interfaces;
    using ServiceLayer;
    using DAL.Interfaces;
    using Common.ValidationConstants;

    public static class Bootstrap
    {
        public static void ConfigureServicesBootstrapper(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddEntityFrameworkSqlServer();

            services.AddDbContext<ApplicationDbContext>
            (
                options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Scoped
            );

            services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
            {
                opts.Password.RequireDigit = UserValidationConstants.DoesUserPasswordRequiresDigit;
                opts.Password.RequireLowercase = UserValidationConstants.DoesUserPasswordRequiresLowercase;
                opts.Password.RequireUppercase = UserValidationConstants.DoesUserPasswordRequiresUppercase;
                opts.Password.RequireNonAlphanumeric = UserValidationConstants.DoesUserPasswordRequiresSymbol;
                opts.Password.RequiredLength = UserValidationConstants.UserPasswordLength;
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IApplicationDbContext, ApplicationDbContext>();
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();

            services.AddAuthentication(opts =>
            {
                opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = configuration["Auth:Jwt:Issuer"],
                    ValidAudience = configuration["Auth:Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(configuration["Auth:Jwt:Key"])),
                    ClockSkew = TimeSpan.Zero,
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true
                };
            });
        }

        public static void ConfigureBoostrapper(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
                var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                dbContext.Database.Migrate();

                DbSeeder.Seed(dbContext, roleManager, userManager);
            }
        }
    }
}