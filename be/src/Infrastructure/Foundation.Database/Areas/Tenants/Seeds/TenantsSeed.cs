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
            var tenantsSeedConfiguration = configuration.GetSection("Seeds")?.GetSection("Tenant");

            if (tenantsSeedConfiguration != null)
            {
                if (!context.Tenants.Any(x => x.Key == "eltap"))
                {
                    var tenant = new Tenant
                    {
                        Key = "eltap",
                        Host = "eltap.com",
                        DatabaseConnectionString = tenantsSeedConfiguration.GetValue<string>("DatabaseConnectionString"),
                        QueueConnectionString = tenantsSeedConfiguration.GetValue<string>("QueueConnectionString"),
                        StorageConnectionString = tenantsSeedConfiguration.GetValue<string>("StorageConnectionString"),
                        IsActive = true,
                        LastModifiedBy = "dawid.dworak@giuru.com",
                        LastModifiedDate = DateTime.UtcNow,
                        CreatedBy = "dawid.dworak@giuru.com",
                        CreatedDate = DateTime.UtcNow
                    };

                    context.Tenants.Add(tenant);
                }

                context.SaveChanges();
            }
        }
    }
}
