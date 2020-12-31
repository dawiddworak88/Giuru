using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading;

namespace Foundation.ApiExtensions.DependencyInjection
{
    public static class ConfigureRoot
    {
        public static void UseLocalization(this ApplicationBuilder app, IConfiguration configuration)
        {
            app.Use((context, next) =>
            {
                var userLangs = context.Request.Headers["Accept-Language"].ToString();

                if (!string.IsNullOrWhiteSpace(userLangs))
                {
                    var firstLang = userLangs.Split(',').FirstOrDefault();

                    if (firstLang != null)
                    {
                        var supportedLanguages = configuration["SupportedLanguages"];

                        if (!string.IsNullOrWhiteSpace(supportedLanguages))
                        {
                            var supportedLanguagesList = supportedLanguages.Split(',');

                            if (supportedLanguages.Contains(firstLang))
                            {
                                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(firstLang);
                                Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
                            }
                        }
                    }
                }

                return next();
            });
        }
    }
}