using Foundation.Database.Areas.Tenants.Entities;
using Foundation.Database.Shared.Contexts;
using System;
using System.Linq;

namespace Foundation.Database.Areas.Tenants.Seeds
{
    public static class TenantsSeed
    {
        public static void SeedTenants(DatabaseContext context)
        {
            if (!context.Tenants.Any(x => x.Name == "eltap"))
            {
                var tenant = new Tenant
                {
                    Name = "eltap",
                    User = context.Accounts.FirstOrDefault(x => x.Email == "szymon.dworak@eltap.com"),
                    IsActive = true,
                    LastModifiedBy = context.Accounts.FirstOrDefault(x => x.Email == "dawid.dworak@giuru.com"),
                    LastModifiedDate = DateTime.UtcNow,
                    CreatedBy = context.Accounts.FirstOrDefault(x => x.Email == "dawid.dworak@giuru.com"),
                    CreatedDate = DateTime.UtcNow
                };

                context.Tenants.Add(tenant);
            }

            context.SaveChanges();
        }
    }
}
