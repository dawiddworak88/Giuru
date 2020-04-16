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
            var adminSeedConfiguration = configuration.GetSection("Seeds")?.GetSection("Accounts")?.GetSection("Admin");


            if (adminSeedConfiguration != null)
            {
                if (!context.Accounts.Any(x => x.Email == adminSeedConfiguration.GetValue<string>("Email")))
                {
                    var adminAccount = new ApplicationUser
                    {
                        FirstName = adminSeedConfiguration.GetValue<string>("FirstName"),
                        LastName = adminSeedConfiguration.GetValue<string>("LastName"),
                        UserName = adminSeedConfiguration.GetValue<string>("Email"),
                        NormalizedUserName = adminSeedConfiguration.GetValue<string>("Email"),
                        Email = adminSeedConfiguration.GetValue<string>("Email"),
                        NormalizedEmail = adminSeedConfiguration.GetValue<string>("Email"),
                        PasswordHash = adminSeedConfiguration.GetValue<string>("PasswordHash"),
                        SecurityStamp = adminSeedConfiguration.GetValue<string>("SecurityStamp"),
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
            var tenantSeedConfiguration = configuration.GetSection("Seeds")?.GetSection("Accounts")?.GetSection("Tenant");

            if (tenantSeedConfiguration != null)
            {
                if (!context.Accounts.Any(x => x.Email == tenantSeedConfiguration.GetValue<string>("Email")))
                {
                    var tenantAccount = new ApplicationUser
                    {
                        FirstName = tenantSeedConfiguration.GetValue<string>("FirstName"),
                        LastName = tenantSeedConfiguration.GetValue<string>("LastName"),
                        UserName = tenantSeedConfiguration.GetValue<string>("Email"),
                        NormalizedUserName = tenantSeedConfiguration.GetValue<string>("Email"),
                        Email = tenantSeedConfiguration.GetValue<string>("Email"),
                        NormalizedEmail = tenantSeedConfiguration.GetValue<string>("Email"),
                        PasswordHash = tenantSeedConfiguration.GetValue<string>("PasswordHash"),
                        SecurityStamp = tenantSeedConfiguration.GetValue<string>("SecurityStamp"),
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
