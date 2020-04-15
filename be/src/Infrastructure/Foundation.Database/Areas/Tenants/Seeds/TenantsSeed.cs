using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Contexts;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Foundation.Database.Areas.Tenants.Seeds
{
    public static class TenantsSeed
    {
        public static void SeedTenants(DatabaseContext context, IConfiguration configuration)
        {
            var adminSeedConfiguration = configuration.GetSection("Seeds")?.GetSection("Accounts")?.GetSection("Admin");
            var tenantsSeedConfiguration = configuration.GetSection("Seeds")?.GetSection("Accounts")?.GetSection("Tenant");

            if (adminSeedConfiguration != null && tenantsSeedConfiguration != null)
            {
                if (!context.Tenants.Any(x => x.Key == tenantsSeedConfiguration.GetValue<string>("Key")))
                {
                    var tenant = new Tenant
                    {
                        Key = tenantsSeedConfiguration.GetValue<string>("Key"),
                        Host = tenantsSeedConfiguration.GetValue<string>("Host"),
                        DatabaseConnectionString = tenantsSeedConfiguration.GetValue<string>("DatabaseConnectionString"),
                        QueueConnectionString = tenantsSeedConfiguration.GetValue<string>("QueueConnectionString"),
                        StorageConnectionString = tenantsSeedConfiguration.GetValue<string>("StorageConnectionString"),
                        IsActive = true,
                        LastModifiedBy = adminSeedConfiguration.GetValue<string>("Email"),
                        LastModifiedDate = DateTime.UtcNow,
                        CreatedBy = adminSeedConfiguration.GetValue<string>("Email"),
                        CreatedDate = DateTime.UtcNow
                    };

                    context.Tenants.Add(tenant);
                }

                context.SaveChanges();
            }
        }
    }
}
