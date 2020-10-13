using Identity.Api.Areas.Accounts.Services.UserServices;
using Identity.Api.Infrastructure.Accounts.Definitions;
using Identity.Api.Infrastructure.Accounts.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Identity.Api.Infrastructure.Accounts.Seeds
{
    public static class AccountsSeed
    {
        public static void SeedAccounts(IdentityContext context, IConfiguration configuration, IUserService userService)
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
                            OrganisationId = Guid.Parse(accountConfiguration[AccountsSeedConstants.OrganisationIdIndex]),
                            SecurityStamp = Guid.NewGuid().ToString(),
                            PhoneNumber = null,
                            PhoneNumberConfirmed = false,
                            TwoFactorEnabled = false,
                            EmailConfirmed = true,
                            AccessFailedCount = 0
                        };

                        sellerAccount.PasswordHash = userService.GeneratePasswordHash(sellerAccount, accountConfiguration[AccountsSeedConstants.PasswordIndex]);

                        context.Accounts.Add(sellerAccount);
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
