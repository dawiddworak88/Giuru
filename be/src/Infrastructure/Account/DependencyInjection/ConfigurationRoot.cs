using Foundation.Account.Definitions;
using Microsoft.AspNetCore.Builder;

namespace Foundation.Account.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void UseAccountIdentityServer(this IApplicationBuilder app)
        {
            app.Use((context, next) =>
            {
                context.Request.Scheme = AccountConstants.HttpsScheme;
                return next();
            });

            app.UseIdentityServer();
        }

        public static void UseAuthenticationAuthorization(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
