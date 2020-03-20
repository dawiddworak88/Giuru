using Microsoft.AspNetCore.Builder;

namespace Feature.Account.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void UseAccountIdentityServer(this IApplicationBuilder app)
        {
            app.UseIdentityServer();

            app.Use((context, next) =>
            {
                context.Request.Scheme = Definitions.Constants.HttpsScheme;
                return next();
            });
        }
    }
}
