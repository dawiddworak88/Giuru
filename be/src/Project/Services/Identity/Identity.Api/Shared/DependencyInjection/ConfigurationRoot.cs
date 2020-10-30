using Microsoft.AspNetCore.Builder;

namespace Identity.Api.Areas.Accounts.DependencyInjection
{
    public static class ConfigurationRoot
    {
        public static void UseAuthenticationAuthorization(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
    }
}
