using Analytics.Api.Infrastructure;
using Analytics.Api.Infrastructure.Entities.SalesAnalytics;
using Analytics.Api.ServicesModels.SalesAnalytics;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
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

        public async Task<IEnumerable<SalesAnalyticsServiceModel>> GetSalesAnalyticsAsync(GetSalesAnalyticsServiceModel model)
        {
            var salesAnalytics = from s in this.context.SalesFacts
                                 join p in this.context.ProductDimensions on s.ProductDimensionId equals p.Id
                                 join t in this.context.TimeDimensions on s.TimeDimensionId equals t.Id
                                 join c in this.context.ClientDimensions on s.ClientDimensionId equals c.Id
                                 where s.IsActive
                                 select new 
                                 {
                                     Id = s.Id,
                                     ProductId = p.ProductId,
                                     ProductSku = p.Sku,
                                     Ean = p.Ean,
                                     ClientId = c.ClientId,
                                     ClientName = c.Name,
                                     Email = c.Email,
                                     IsOutlet = s.IsOutlet,
                                     IsStock = s.IsStock,
                                     Quantity = s.Quantity
                                 };

            if (model.IsSeller is false)
            {
                salesAnalytics = salesAnalytics.Where(x => x.Email == model.Username);
            }

            var productGroups = salesAnalytics.OrEmptyIfNull().GroupBy(g => g.ProductId);

            var salesAnalyticsItems = new List<SalesAnalyticsServiceModel>();

            foreach (var productGroup in productGroups.OrEmptyIfNull())
            {
                var salesAnalyticsItem = new SalesAnalyticsServiceModel
                {
                    Id = productGroup.FirstOrDefault().Id,
                    ProductId = productGroup.FirstOrDefault().ProductId,
                    ProductSku = productGroup.FirstOrDefault().ProductSku,
                    Ean = productGroup.FirstOrDefault().Ean,
                    Quantity = productGroup.Sum(x => x.Quantity)
                };

                salesAnalyticsItems.Add(salesAnalyticsItem);
            }

            return salesAnalyticsItems;
        }
    }
}
