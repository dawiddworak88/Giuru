using Catalog.Api.Infrastructure.Products.Definitions;
using Foundation.Catalog.Infrastructure;
using Foundation.Catalog.Infrastructure.Products.Entities;
using Foundation.GenericRepository.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

namespace Catalog.Api.Infrastructure.Products.Seeds
{
    public static class BrandsSeed
    {
        public static void SeedBrands(CatalogContext context, IConfiguration configuration)
        {
            var brands = configuration["Brands"]?.Split(";");

            if (brands != null)
            {
                foreach (var brand in brands)
                {
                    var brandConfiguration = brand.Split("&");

                    SeedBrand(context, Guid.Parse(brandConfiguration[BrandsSeedConstants.IdIndex]), brandConfiguration[BrandsSeedConstants.NameIndex], Guid.Parse(brandConfiguration[BrandsSeedConstants.SellerIdIndex]));
                }
            }
        }

        private static void SeedBrand(CatalogContext context, Guid id, string name, Guid sellerId)
        {
            if (!context.Brands.Any(x => x.Id == id))
            {
                var brand = new Brand
                {
                    Id = id,
                    Name = name,
                    SellerId = sellerId
                };

                context.Brands.Add(brand.FillCommonProperties());

                context.SaveChanges();
            }
        }
    }
}
