using Identity.Api.Infrastructure.Accounts.Entities;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Identity.Api.Infrastructure.Accounts.Seeds
{
    public static class AccountsSeed
    {
        public static void SeedAdminAccounts(IdentityContext context, IConfiguration configuration)
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

        public static void SeedSellerAccounts(IdentityContext context, IConfiguration configuration)
        {
            var sellerSeedConfiguration = configuration.GetSection("Seeds")?.GetSection("Accounts")?.GetSection("Seller");

            if (sellerSeedConfiguration != null)
            {
                if (!context.Accounts.Any(x => x.Email == sellerSeedConfiguration.GetValue<string>("Email")))
                {
                    var sellerAccount = new ApplicationUser
                    {
                        FirstName = sellerSeedConfiguration.GetValue<string>("FirstName"),
                        LastName = sellerSeedConfiguration.GetValue<string>("LastName"),
                        UserName = sellerSeedConfiguration.GetValue<string>("Email"),
                        NormalizedUserName = sellerSeedConfiguration.GetValue<string>("Email"),
                        Email = sellerSeedConfiguration.GetValue<string>("Email"),
                        NormalizedEmail = sellerSeedConfiguration.GetValue<string>("Email"),
                        PasswordHash = sellerSeedConfiguration.GetValue<string>("PasswordHash"),
                        SecurityStamp = sellerSeedConfiguration.GetValue<string>("SecurityStamp"),
                        // Seller = context.Sellers.FirstOrDefault(x => x.Name == sellerSeedConfiguration.GetValue<string>("Name")),
                        PhoneNumber = null,
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        EmailConfirmed = true,
                        AccessFailedCount = 0
                    };

                    context.Accounts.Add(sellerAccount);
                }

                context.SaveChanges();
            }
        }
    }
}
