using Identity.Api.Infrastructure.Accounts.Definitions;
using Identity.Api.Infrastructure.Accounts.Entities;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Identity.Api.Infrastructure.Accounts.Seeds
{
    public static class AccountsSeed
    {
        public static void SeedAccounts(IdentityContext context, IConfiguration configuration)
        {
            var accounts = configuration["Accounts"]?.Split(";");

            if (accounts != null)
            {
                foreach (var account in accounts)
                {
                    var accountConfiguration = account.Split("&");

                    if (!context.Accounts.Any(x => x.Email == accountConfiguration[AccountsSeedConstants.EmailIndex]))
                    {
                        var sellerAccount = new ApplicationUser
                        {
                            FirstName = accountConfiguration[AccountsSeedConstants.FirstNameIndex],
                            LastName = accountConfiguration[AccountsSeedConstants.LastNameIndex],
                            UserName = accountConfiguration[AccountsSeedConstants.EmailIndex],
                            NormalizedUserName = accountConfiguration[AccountsSeedConstants.EmailIndex],
                            Email = accountConfiguration[AccountsSeedConstants.EmailIndex],
                            NormalizedEmail = accountConfiguration[AccountsSeedConstants.EmailIndex],
                            PasswordHash = accountConfiguration[AccountsSeedConstants.PasswordHashIndex],
                            SecurityStamp = accountConfiguration[AccountsSeedConstants.SecurityStampIndex],
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
}
