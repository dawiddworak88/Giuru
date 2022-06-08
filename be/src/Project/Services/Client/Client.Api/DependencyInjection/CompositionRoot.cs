using Client.Api.Infrastructure;
using Client.Api.Services.Clients;
using Client.Api.Services.Groups;
using Client.Api.Services.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Client.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterClientApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<IClientsService, ClientsService>();
            services.AddScoped<IClientGroupsService, ClientGroupsService>();
            services.AddScoped<IClientManagersService, ClientManagersService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ClientContext>();

            services.AddDbContext<ClientContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }
    }
}
