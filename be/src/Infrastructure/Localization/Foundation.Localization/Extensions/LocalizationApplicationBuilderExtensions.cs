using Foundation.Localization.Definitions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Foundation.Localization.Extensions
{
    public static class LocalizationApplicationBuilderExtensions
    {
        public static void UseCustomRequestLocalizationProvider(
            this IApplicationBuilder applicationBuilder,
            IOptionsMonitor<LocalizationSettings> localizationConfiguration)
        {
            const int First = 0;
            const int Second = 1;

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(localizationConfiguration.CurrentValue.DefaultCulture),
                SupportedCultures = GetSupportedCultures(localizationConfiguration.CurrentValue.SupportedCultures),
                SupportedUICultures = GetSupportedCultures(localizationConfiguration.CurrentValue.SupportedCultures)
            };

            var routeRequestProvider = new RouteDataRequestCultureProvider();
            localizationOptions.RequestCultureProviders.Insert(First, routeRequestProvider);

            var acceptLanguageRequestProvider = new AcceptLanguageHeaderRequestCultureProvider();
            localizationOptions.RequestCultureProviders.Insert(Second, acceptLanguageRequestProvider);

            applicationBuilder.UseRequestLocalization(localizationOptions);
        }

        private static IList<CultureInfo> GetSupportedCultures(string supportedCultures) =>
            supportedCultures
                .Split(',')
                .Select(supportedCulture => new CultureInfo(supportedCulture))
                .ToList();
    }
}
