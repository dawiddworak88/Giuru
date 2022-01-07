using Foundation.Localization.Definitions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;

namespace Foundation.Localization.Extensions
{
    public static class LocalizationApplicationBuilderExtensions
    {
        public static void UseCustomHeaderRequestLocalizationProvider(
            this IApplicationBuilder app,
            IConfiguration configuration,
            IOptionsMonitor<LocalizationSettings> localizationConfiguration)
        {
            app.Use((context, next) =>
            {
                var acceptLanguages = context.Request.Headers["Accept-Language"].ToString();

                if (!string.IsNullOrWhiteSpace(acceptLanguages))
                {
                    var lang = acceptLanguages.Split(',').FirstOrDefault();

                    if (lang != null)
                    {
                        if (lang.Contains("-"))
                        {
                            lang = lang.Split("-").FirstOrDefault();
                        }

                        if (localizationConfiguration.CurrentValue.SupportedCultures.Split(',').Contains(lang))
                        {
                            Thread.CurrentThread.CurrentCulture = new CultureInfo(lang);
                            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                        }
                        else
                        {
                            Thread.CurrentThread.CurrentCulture = new CultureInfo(configuration["DefaultCulture"]);
                            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                        }
                    }
                }
                else
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(configuration["DefaultCulture"]);
                    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                }

                return next();
            });
        }

        public static void UseCustomRouteRequestLocalizationProvider(
            this IApplicationBuilder applicationBuilder,
            IOptionsMonitor<LocalizationSettings> localizationConfiguration)
        {
            const int First = 0;

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(localizationConfiguration.CurrentValue.DefaultCulture),
                SupportedCultures = GetSupportedCultures(localizationConfiguration.CurrentValue.SupportedCultures),
                SupportedUICultures = GetSupportedCultures(localizationConfiguration.CurrentValue.SupportedCultures)
            };

            var routeRequestProvider = new RouteDataRequestCultureProvider();
            localizationOptions.RequestCultureProviders.Insert(First, routeRequestProvider);

            applicationBuilder.UseRequestLocalization(localizationOptions);
        }

        private static IList<CultureInfo> GetSupportedCultures(string supportedCultures) =>
            supportedCultures
                .Split(',')
                .Select(supportedCulture => new CultureInfo(supportedCulture))
                .ToList();
    }
}
