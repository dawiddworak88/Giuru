using Foundation.Localization.Definitions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Localization.Routing;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Foundation.Localization.Extensions
{
    public static class LocalizationApplicationBuilderExtensions
    {
        public static void UseRequestLocalizationWithRouteCultureProvider(
            this IApplicationBuilder applicationBuilder,
            LocalizationConfiguration options)
        {
            const int First = 0;

            var localizationOptions = new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(options.DefaultRequestCulture),
                SupportedCultures = GetSupportedCultures(options),
                SupportedUICultures = GetSupportedCultures(options)
            };

            var requestProvider = new RouteDataRequestCultureProvider();
            localizationOptions.RequestCultureProviders.Insert(First, requestProvider);

            applicationBuilder.UseRequestLocalization(localizationOptions);
        }

        private static IList<CultureInfo> GetSupportedCultures(LocalizationConfiguration options) =>
            options.SupportedCultures
                .Select(supportedCulture => new CultureInfo(supportedCulture))
                .ToList();
    }
}
