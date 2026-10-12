using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.Configurations;
using System;

namespace Seller.Web.Shared.Filters
{
    /// <summary>
    /// Makes a controller reachable only while Grula is configured: discount codes are Grula price drivers, so without
    /// Grula the feature does not exist. Hiding the menu entry does not protect typed URLs or API requests, so the
    /// check is enforced here with a 404.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RequireGrulaAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var options = context.HttpContext.RequestServices.GetRequiredService<IOptions<AppSettings>>();

            if (options.Value.IsGrulaConfigured is false)
            {
                context.Result = new NotFoundResult();
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}
