using Foundation.Database.Areas.Accounts.Entities;
using Foundation.Database.Shared.Contexts;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Foundation.Database.Areas.Accounts.Seeds
{
    public static class AccountsSeed
    {
        public static void SeedAdminAccounts(DatabaseContext context, IConfiguration configuration)
        {
            var accountsSeedConfiguration = configuration.GetSection("Seeds")?.GetSection("Accounts");

            if (accountsSeedConfiguration != null)
            {
                if (!context.Accounts.Any(x => x.Email == "dawid.dworak@giuru.com"))
                {
                    var adminAccount = new ApplicationUser
                    {
                        FirstName = "Dawid",
                        LastName = "Dworak",
                        UserName = "dawid.dworak@giuru.com",
                        NormalizedUserName = "dawid.dworak@giuru.com",
                        Email = "dawid.dworak@giuru.com",
                        NormalizedEmail = "dawid.dworak@giuru.com",
                        PasswordHash = accountsSeedConfiguration.GetValue<string>("AdminHash"),
                        SecurityStamp = "d2a44f96-c29c-4f71-9de2-41219318d668",
                        PhoneNumber = null,
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        EmailConfirmed = true,
                        AccessFailedCount = 0
                    };

                    context.Accounts.Add(adminAccount);
                }

                context.SaveChanges();
            }
        }

        public static void SeedTenantAccounts(DatabaseContext context, IConfiguration configuration)
        {
            var accountsSeedConfiguration = configuration.GetSection("Seeds")?.GetSection("Accounts");

            if (accountsSeedConfiguration != null)
            {
                if (!context.Accounts.Any(x => x.Email == "szymon.dworak@eltap.com"))
                {
                    var tenantAccount = new ApplicationUser
                    {
                        FirstName = "Szymon",
                        LastName = "Dworak",
                        UserName = "szymon.dworak@eltap.com",
                        NormalizedUserName = "szymon.dworak@eltap.com",
                        Email = "szymon.dworak@eltap.com",
                        NormalizedEmail = "szymon.dworak@eltap.com",
                        PasswordHash = accountsSeedConfiguration.GetValue<string>("TenantHash"),
                        Tenant = context.Tenants.FirstOrDefault(x => x.Key == "eltap"),
                        SecurityStamp = "8d241f34-de16-40dc-b887-373f615b610f",
                        PhoneNumber = null,
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        EmailConfirmed = true,
                        AccessFailedCount = 0
                    };

                    context.Accounts.Add(tenantAccount);
                }

                context.SaveChanges();
            }
        }
    }
}
