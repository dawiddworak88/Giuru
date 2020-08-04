using Foundation.Security.Definitions;
using Microsoft.AspNetCore.Builder;

namespace Foundation.Security.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void UseSecurityHeaders(this IApplicationBuilder app)
        {
            app.UseHsts(options => options.MaxAge(days: SecurityConstants.HstsMaxAgeInDays));
            app.UseXContentTypeOptions();
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.SameOrigin());
            app.UseReferrerPolicy(opts => opts.NoReferrerWhenDowngrade());

            app.UseCsp(options => options
                .DefaultSources(s => s.Self()
                    .CustomSources("data:")
                    .CustomSources("https:"))
                .StyleSources(s => s.Self()
                    .CustomSources("www.google.com", "fonts.googleapis.com")
                    .UnsafeInline()
                )
                .ScriptSources(s => s.Self()
                       .CustomSources("www.google.com")
                    .UnsafeInline()
                    .UnsafeEval()
                )
            );

            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("Feature-Policy", "geolocation 'none';midi 'none';notifications 'none';push 'none';sync-xhr 'none';microphone 'none';camera 'none';magnetometer 'none';gyroscope 'none';speaker 'self';vibrate 'none';fullscreen 'self';payment 'none';");
                await next.Invoke();
            });
        }
    }
}
