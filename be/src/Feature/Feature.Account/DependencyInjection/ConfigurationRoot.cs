using Microsoft.AspNetCore.Builder;

namespace Feature.Account.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void UseAccountIdentityServer(this IApplicationBuilder app)
        {
            app.UseIdentityServer();
        }
    }
}
