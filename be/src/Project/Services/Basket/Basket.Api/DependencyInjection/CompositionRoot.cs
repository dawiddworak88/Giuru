using Basket.Api.Repositories;
using Basket.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Basket.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterBasketApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<IBasketRepository, RedisBasketRepository>();
            services.AddScoped<IBasketService, BasketService>();
        }
    }
}
