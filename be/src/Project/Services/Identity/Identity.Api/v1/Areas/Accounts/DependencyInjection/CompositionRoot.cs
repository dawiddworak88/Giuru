using Identity.Api.v1.Areas.Accounts.Services.Organisations;
using Identity.Api.v1.Areas.Accounts.Services.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.Api.v1.Areas.Accounts.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterAccountsApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<ISellerService, SellerService>();
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
