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
using System.Security.Cryptography.X509Certificates;

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

        public async Task<IEnumerable<TopSalesProductsAnalyticsServiceModel>> GetTopSalesProductsAnalyticsAsync(GetTopSalesProductsAnalyticsServiceModel model)
        {
            var topProducts = from s in this.context.SalesFacts
                               join p in this.context.ProductDimensions on s.ProductDimensionId equals p.Id
                               where s.IsActive
                               group s by new { p.ProductId } into gp
                               where gp.Sum(x => x.Quantity) > 0
                               select new
                               {
                                   Id = gp.FirstOrDefault().ProductDimensionId,
                                   ProductId = gp.Key.ProductId,
                                   ProductSku = this.context.ProductDimensions.FirstOrDefault(x => x.ProductId == gp.Key.ProductId && x.IsActive).Sku,
                                   ProductName = this.context.ProductTranslationDimensions.FirstOrDefault(x => x.ProductDimensionId == gp.FirstOrDefault().ProductDimensionId && x.IsActive).Name,
                                   Ean = this.context.ProductDimensions.FirstOrDefault(x => x.ProductId == gp.Key.ProductId && x.IsActive).Ean,
                                   Email = this.context.ClientDimensions.FirstOrDefault(x => x.Id == gp.FirstOrDefault().ClientDimensionId && x.IsActive).Email,
                                   Quantity = gp.Sum(y => y.Quantity)
                               };

            if (model.IsSeller is false)
            {
                topProducts = topProducts.Where(x => x.Email == model.Username);
            }

            var topSalesProducts = new List<TopSalesProductsAnalyticsServiceModel>();

            foreach (var topProduct in topProducts.OrEmptyIfNull())
            {
                var topSalesProduct = new TopSalesProductsAnalyticsServiceModel
                {
                    Id = topProduct.Id,
                    ProductId = topProduct.ProductId,
                    ProductSku = topProduct.ProductSku,
                    ProductName = topProduct.ProductName,
                    Ean = topProduct.Ean,
                    Quantity = topProduct.Quantity
                };

                topSalesProducts.Add(topSalesProduct);
            }

            return topSalesProducts.OrderByDescending(x => x.Quantity);
        }
    }
}
