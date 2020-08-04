using Foundation.Account.Services;
using Foundation.Account.UserStores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Account.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterBaseAccountDependencies(this IServiceCollection services)
        {
            services.AddScoped<UserStoreFactory>();
            services.AddScoped<IPasswordGenerationService, PasswordGenerationService>();
            services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
        }
    }
}
