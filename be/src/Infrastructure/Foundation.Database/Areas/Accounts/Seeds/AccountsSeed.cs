using Foundation.Database.Areas.Accounts.Entities;
using Foundation.Database.Shared.Contexts;
using System.Linq;

namespace Foundation.Database.Areas.Accounts.Seeds
{
    public static class AccountsSeed
    {
        #pragma warning disable 2068
        public static void SeedAccounts(DatabaseContext context)
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
                    PasswordHash = "AHhb6UTPYPHtRPgBrYozbr+82Cn/QrOz8KlyLcsoHtkUu5Ht7HX2XkfAQUAR6WTudw==",
                    SecurityStamp = "d2a44f96-c29c-4f71-9de2-41219318d668",
                    PhoneNumber = null,
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    EmailConfirmed = true,
                    AccessFailedCount = 0
                };

                context.Accounts.Add(adminAccount);
            }

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
                    PasswordHash = "AI/jeihvMxIoInb6SIDwYMJpJUU53oxnq0tnLMzwoSEp/qfp15xDOKZh27IwYR0iTQ==",
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
        #pragma warning restore 2068
    }
}
