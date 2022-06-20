using Client.Api.Infrastructure;
using Client.Api.Services.Applications;
using Client.Api.Services.Clients;
using Client.Api.Services.Groups;
using Client.Api.Services.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Reflection;

namespace Client.Api.DependencyInjection
{
    public static class CompositionRoot
    {
        public static void RegisterClientApiDependencies(this IServiceCollection services)
        {
            services.AddScoped<IClientsService, ClientsService>();
            services.AddScoped<IClientGroupsService, ClientGroupsService>();
            services.AddScoped<IClientsApplicationsService, ClientsApplicationsService>();
            services.AddScoped<IClientAccountManagersService, ClientAccountManagersService>();
        }

        public static void RegisterDatabaseDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<ClientContext>();

            services.AddDbContext<ClientContext>(options => options.UseSqlServer(configuration["ConnectionString"], opt => opt.UseNetTopologySuite()));
        }
    }
}
