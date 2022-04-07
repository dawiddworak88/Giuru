using Foundation.Security.Definitions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Foundation.Security.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void UseSecurityHeaders(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseHsts(options => options.MaxAge(days: SecurityConstants.HstsMaxAgeInDays));
            app.UseXContentTypeOptions();
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.SameOrigin());
            app.UseReferrerPolicy(opts => opts.NoReferrerWhenDowngrade());

            app.UseCsp(options => options
                .DefaultSources(s => s.Self()
                    .CustomSources("data:", "https:", "http:"))
                .StyleSources(s => s.Self()
                    .CustomSources("*")
                    .UnsafeInline()
                )
                .ScriptSources(s => s.Self()
                       .CustomSources("*")
                    .UnsafeInline()
                    .UnsafeEval()
                )
                .ImageSources(s => s.Self()
                    .CustomSources("data:", "https", "http:")));

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Feature-Policy", "geolocation 'none';midi 'none';sync-xhr 'none';microphone 'none';camera 'none';magnetometer 'none';gyroscope 'none';fullscreen 'self';payment 'none';");
                await next.Invoke();
            });
        }
    }
}
