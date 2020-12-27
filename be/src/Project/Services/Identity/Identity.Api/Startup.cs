using Foundation.Localization.Definitions;
using Foundation.Localization.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Identity.Api.Shared.DependencyInjection;
using Foundation.Extensions.DependencyInjection;
using Foundation.PageContent.DependencyInjection;
using Foundation.Security.DependencyInjection;
using Foundation.Account.DependencyInjection;
using Foundation.GenericRepository.DependencyInjection;
using Foundation.Extensions.Definitions;
using Identity.Api.Infrastructure.DependencyInjection;
using Identity.Api.Areas.Accounts.Services.UserServices;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System;
using Identity.Api.Areas.Accounts.DependencyInjection;
using Identity.Api.v1.Areas.Accounts.DependencyInjection;
using Identity.Api.Areas.Home.DependencyInjection;
using Identity.Api.v1.Areas.Clients.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace Account
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment currentEnvironment)
        {
            Configuration = configuration;
            CurrentEnvironment = currentEnvironment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment CurrentEnvironment { get;  }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLocalizationConfiguration(this.Configuration);

            services.AddLocalization();

            services.AddCultureRouteConstraint();

            services.AddControllersWithViews();

            services.RegisterLocalizationDependencies();

            services.RegisterDatabaseDependencies(this.Configuration);

            services.RegisterBaseAccountDependencies();

            services.RegisterAccountDependencies(this.Configuration, this.CurrentEnvironment.EnvironmentName != EnvironmentConstants.DevelopmentEnvironmentName);
            
            services.RegisterApiAccountDependencies(this.Configuration);
            
            services.RegisterGeneralDependencies();

            services.RegisterAccountsViewsDependencies();

            services.RegisterAccountsApiDependencies();

            services.RegisterHomeViewsDependencies();
            
            services.RegisterClientsApiDependencies();

            services.ConfigureGenericRepositoryOptions(this.Configuration);

            services.ConfigureOptions(this.Configuration);

            services.AddApiVersioning();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Identity API", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptionsMonitor<LocalizationConfiguration> localizationOptions, IUserService userService)
        {
            IdentityModelEventSource.ShowPII = true;

            app.UseForwardedHeaders();

            app.UseGeneralException();

            app.UseResponseCompression();

            app.UseGeneralStaticFiles();

            app.ConfigureDatabaseMigrations(this.Configuration, userService);

            app.UseRouting();

            app.UseIdentityServer();

            app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Lax });

            app.UseAuthorization();

            app.UseRequestLocalizationWithRouteCultureProvider(localizationOptions.CurrentValue);

            app.UseSecurityHeaders(this.Configuration);

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity API");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                            name: "localizedAreaRoute",
                            pattern: "{culture:" + LocalizationConstants.CultureRouteConstraint + "}/{area:exists=Accounts}/{controller=SignIn}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{area:exists=Accounts}/{controller=SignIn}/{action=Index}/{id?}");
            });
        }
    }
}