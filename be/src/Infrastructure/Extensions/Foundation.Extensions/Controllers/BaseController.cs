using Foundation.Localization.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace Foundation.Extensions.Controllers
{
    [LocalizationCookieFilter]
    public class BaseController : Controller
    {
        protected string CurrentLanguage => CultureInfo.CurrentUICulture.Name;

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var routeValues = context.RouteData.Values;

            if (routeValues != null && !routeValues.ContainsKey("culture"))
            {
                routeValues.Add("culture", CurrentLanguage);

                foreach (var queryItem in this.Request.Query)
                {
                    routeValues.Add(queryItem.Key, queryItem.Value);
                }

                context.Result = new RedirectToRouteResult(routeValues);
            }
        }
    }
}
