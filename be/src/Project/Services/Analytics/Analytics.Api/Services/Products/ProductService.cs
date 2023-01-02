using Analytics.Api.Infrastructure;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace Analytics.Api.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly AnalyticsContext context;

        public ProductService(
            AnalyticsContext context)
        {
            this.context = context;
        }

        public async Task UpdateProductAsync(Guid? productId, string productName, string productSku, string productEan, string language)
        {
            var product = await this.context.ProductDimensions.FirstOrDefaultAsync(x => x.Id == productId.Value && x.IsActive);

            if (product is not null)
            {
                product.Sku = productSku;
                product.Ean = productEan;
                product.LastModifiedDate = DateTime.UtcNow;

                var productTranslation = await this.context.ProductTranslationDimensions.FirstOrDefaultAsync(x => x.ProductDimensionId == product.Id && x.IsActive && x.Language == language);

                if (productTranslation is not null)
                {
                    productTranslation.Name = productName;
                }

                await this.context.SaveChangesAsync();
            }
        }
    }
}
