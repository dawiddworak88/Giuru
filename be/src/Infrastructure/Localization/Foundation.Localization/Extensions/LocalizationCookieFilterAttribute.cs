using Foundation.Localization.Definitions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Localization.Extensions
{
    public class LocalizationCookieFilterAttribute : Attribute, IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            var language = CultureInfo.CurrentCulture.Name;

            if (language != null && CultureExists(language))
            {
                context.HttpContext.Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(language)),
                    new CookieOptions { Expires = LocalizationConstants.ExpirationDateOfLocalizationCookie, HttpOnly = true, Secure = true });
            }

            await next();
        }

        private bool CultureExists(string cultureName) =>
            CultureInfo.GetCultures(CultureTypes.AllCultures)
                       .Any(culture => string.Equals(culture.Name,
                                                     cultureName,
                                                     StringComparison.CurrentCultureIgnoreCase));
    }
}
