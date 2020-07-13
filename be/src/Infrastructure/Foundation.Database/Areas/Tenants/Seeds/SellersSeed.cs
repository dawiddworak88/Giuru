using Foundation.Database.Areas.Sellers.Entities;
using Foundation.Database.Shared.Contexts;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Foundation.Database.Areas.Sellers.Seeds
{
    public static class SellersSeed
    {
        public static void SeedSellers(DatabaseContext context, IConfiguration configuration)
        {
            var adminSeedConfiguration = configuration.GetSection("Seeds")?.GetSection("Accounts")?.GetSection("Admin");
            var sellersSeedConfiguration = configuration.GetSection("Seeds")?.GetSection("Accounts")?.GetSection("Seller");

            if (adminSeedConfiguration != null && sellersSeedConfiguration != null)
            {
                if (!context.Sellers.Any(x => x.Key == sellersSeedConfiguration.GetValue<string>("Key")))
                {
                    var seller = new Seller
                    {
                        Key = sellersSeedConfiguration.GetValue<string>("Key"),
                        Host = sellersSeedConfiguration.GetValue<string>("Host"),
                        DatabaseConnectionString = sellersSeedConfiguration.GetValue<string>("DatabaseConnectionString"),
                        QueueConnectionString = sellersSeedConfiguration.GetValue<string>("QueueConnectionString"),
                        StorageConnectionString = sellersSeedConfiguration.GetValue<string>("StorageConnectionString"),
                        IsActive = true,
                        LastModifiedBy = adminSeedConfiguration.GetValue<string>("Email"),
                        LastModifiedDate = DateTime.UtcNow,
                        CreatedBy = adminSeedConfiguration.GetValue<string>("Email"),
                        CreatedDate = DateTime.UtcNow
                    };

                    context.Sellers.Add(seller);
                }

                context.SaveChanges();
            }
        }
    }
}
