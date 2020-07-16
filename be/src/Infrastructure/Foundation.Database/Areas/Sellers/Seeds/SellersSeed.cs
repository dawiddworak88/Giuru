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
                if (!context.Sellers.Any(x => x.Name == sellersSeedConfiguration.GetValue<string>("Name")))
                {
                    var seller = new Seller
                    {
                        IsActive = true,
                        Name = sellersSeedConfiguration.GetValue<string>("Name"),
                        LastModifiedDate = DateTime.UtcNow,
                        CreatedDate = DateTime.UtcNow
                    };

                    context.Sellers.Add(seller);
                }

                context.SaveChanges();
            }
        }
    }
}
