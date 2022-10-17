using Analytics.Api.Infrastructure;
using Analytics.Api.Infrastructure.Entities.SalesAnalytics;
using Analytics.Api.ServicesModels.SalesAnalytics;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Analytics.Api.Services.SalesAnalytics
{
    public class SalesService : ISalesService
    {
        private readonly AnalyticsContext context;

        public SalesService(
            AnalyticsContext context)
        {
            this.context = context;
        }

        public async Task CreateSalesAnalyticsAsync(CreateSalesAnalyticsServiceModel model)
        {
            foreach (var salesAnalyticsItem in model.SalesAnalyticsItems.OrEmptyIfNull())
            {
                var timeDimension = new TimeDimension
                {
                    Hour = Convert.ToInt32(salesAnalyticsItem.CreatedDate.Value.ToString("HH")),
                    Minute = Convert.ToInt32(salesAnalyticsItem.CreatedDate.Value.ToString("mm")),
                    Second = Convert.ToInt32(salesAnalyticsItem.CreatedDate.Value.ToString("ss")),
                    DayOfWeek = Convert.ToInt32(salesAnalyticsItem.CreatedDate.Value.DayOfWeek),
                    Day = Convert.ToInt32(salesAnalyticsItem.CreatedDate.Value.ToString("dd")),
                    Quarter = (Convert.ToInt32(salesAnalyticsItem.CreatedDate.Value.ToString("MM")) + 2) / 3,
                    Month = Convert.ToInt32(salesAnalyticsItem.CreatedDate.Value.ToString("MM")),
                    Year = Convert.ToInt32(salesAnalyticsItem.CreatedDate.Value.ToString("yyyy"))
                };

                await this.context.TimeDimensions.AddAsync(timeDimension.FillCommonProperties());

                var productDimension = await this.context.ProductDimensions.FirstOrDefaultAsync(x => x.Sku == salesAnalyticsItem.ProductSku);

                if (productDimension is null)
                {
                    productDimension = new ProductDimension
                    {
                        ProductId = salesAnalyticsItem.ProductId.Value,
                        Sku = salesAnalyticsItem.ProductSku,
                        Ean = salesAnalyticsItem.Ean
                    };

                    await this.context.ProductDimensions.AddAsync(productDimension.FillCommonProperties());

                    var productDimensionTranslation = new ProductTranslationDimension
                    {
                        ProductDimensionId = productDimension.Id,
                        Name = salesAnalyticsItem.ProductName,
                        Language = model.Language
                    };

                    await this.context.ProductTranslationDimensions.AddAsync(productDimensionTranslation.FillCommonProperties());
                }

                var clientDimension = await this.context.ClientDimensions.FirstOrDefaultAsync(x => x.ClientId == salesAnalyticsItem.ClientId);

                if (clientDimension is null)
                {
                    clientDimension = new ClientDimension
                    {
                        ClientId = salesAnalyticsItem.ClientId.Value,
                        Email = salesAnalyticsItem.Email,
                        Name = salesAnalyticsItem.ClientName
                    };
                    
                    await this.context.ClientDimensions.AddAsync(clientDimension.FillCommonProperties());
                }

                var salesFact = new SalesFact
                {
                    TimeDimensionId = timeDimension.Id,
                    ClientDimensionId = clientDimension.Id,
                    ProductDimensionId = productDimension.Id,
                    IsOutlet = salesAnalyticsItem.IsOutlet,
                    IsStock = salesAnalyticsItem.IsStock,
                    Quantity = salesAnalyticsItem.Quantity
                };

                await this.context.SalesFacts.AddAsync(salesFact.FillCommonProperties());
                await this.context.SaveChangesAsync();
            }
        }
    }
}
