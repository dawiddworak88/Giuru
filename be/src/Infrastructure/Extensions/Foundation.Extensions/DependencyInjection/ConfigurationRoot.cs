using Foundation.Extensions.Definitions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Net.Http.Headers;

namespace Foundation.Extensions.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void UseGeneralStaticFiles(this IApplicationBuilder app)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                {
                    ctx.Context.Response.Headers[HeaderNames.CacheControl] =
                        "public,max-age=" + CacheControlConstants.CacheControlMaxAgeSeconds;
                }
            });
        }

        public static void UseGeneralException(this IApplicationBuilder app)
        {
            app.UseExceptionHandler("/Home/Error");
        }
    }
}
