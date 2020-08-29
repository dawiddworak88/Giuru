using Microsoft.AspNetCore.Builder;

namespace Foundation.Account.DependencyInjection
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
