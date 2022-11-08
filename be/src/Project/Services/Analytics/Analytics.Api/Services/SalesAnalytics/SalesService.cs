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
                var salesFact = new SalesFact
                {
                    IsOutlet = salesAnalyticsItem.IsOutlet,
                    IsStock = salesAnalyticsItem.IsStock,
                    Quantity = salesAnalyticsItem.Quantity
                };

                await this.context.SalesFacts.AddAsync(salesFact.FillCommonProperties());

                var timeDimension = new TimeDimension
                {
                    SalesFactId = salesFact.Id,
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
                        SalesFactId = salesFact.Id,
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
                        SalesFactId = salesFact.Id,
                        OrganisationId = model.OrganisationId.Value,
                        ClientId = salesAnalyticsItem.ClientId.Value,
                        Email = salesAnalyticsItem.Email,
                        Name = salesAnalyticsItem.ClientName
                    };
                    
                    await this.context.ClientDimensions.AddAsync(clientDimension.FillCommonProperties());
                }

                var locationDimension = await this.context.LocationDimensions.FirstOrDefaultAsync(x => x.Country == salesAnalyticsItem.Country);

                if (locationDimension is null)
                {
                    locationDimension = new LocationDimension
                    {
                        SalesFactId = salesFact.Id,
                        Country = salesAnalyticsItem.Country
                    };

                    await this.context.LocationDimensions.AddAsync(locationDimension.FillCommonProperties());
                }

                await this.context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AnnualProductSalesServiceModel>> GetAnnualProductSalesAsync(GetAnnualProductSalesServiceModel model)
        {
            var sales = from s in this.context.SalesFacts
                          join t in this.context.TimeDimensions on s.Id equals t.SalesFactId
                          join c in this.context.ClientDimensions on s.Id equals c.SalesFactId
                          where s.IsActive && t.IsActive
                          select new
                          {
                              Year = t.Year,
                              Month = t.Month,
                              Quantity = s.Quantity,
                              OrganisationId = c.OrganisationId
                          };

            if (model.IsSeller is false)
            {
                sales = sales.Where(x => x.OrganisationId == model.OrganisationId);
            }

            var now = DateTime.UtcNow;

            var months = Enumerable.Range(-12, 12)
                .Select(x => new    
                    {
                        Year = now.AddMonths(x).Year,
                        Month = now.AddMonths(x).Month
                    });

            var annualSales = months.GroupJoin(sales, 
                m => new 
                    { 
                        Month = m.Month, 
                        Year = m.Year 
                    }, 
                rev => new
                    {
                        Month = rev.Month,
                        Year = rev.Year
                    }, 
                (s, g) => new AnnualProductSalesServiceModel 
                    { 
                        Month = s.Month, 
                        Year = s.Year, 
                        Quantity = g.Sum(x => x.Quantity)
                    }).OrderBy(x => x.Year).ThenBy(x => x.Month);

            return annualSales;
        }

        public async Task<IEnumerable<TopSalesProductsAnalyticsServiceModel>> GetTopSalesProductsAnalyticsAsync(GetTopSalesProductsAnalyticsServiceModel model)
        {
            var topProducts = from s in this.context.SalesFacts
                              join p in this.context.ProductDimensions on s.Id equals p.SalesFactId
                              where s.IsActive && p.IsActive
                              group s by new { p.ProductId } into gp
                              where gp.Sum(x => x.Quantity) > 0
                              select new
                              {
                                  ProductId = gp.Key.ProductId,
                                  ProductSku = gp.First().ProductDimensions.First().Sku,
                                  Ean = gp.First().ProductDimensions.First().Ean,
                                  OrganisationId = this.context.ClientDimensions.FirstOrDefault(x => x.SalesFactId == gp.FirstOrDefault().Id && x.IsActive).OrganisationId,
                                  Quantity = gp.Sum(y => y.Quantity)
                              };

            if (model.IsSeller is false)
            {
                topProducts = topProducts.Where(x => x.OrganisationId == model.OrganisationId);
            }

            var topSalesProducts = new List<TopSalesProductsAnalyticsServiceModel>();

            foreach (var topProduct in topProducts.OrEmptyIfNull())
            {
                var topSalesProduct = new TopSalesProductsAnalyticsServiceModel
                {
                    ProductId = topProduct.ProductId,
                    ProductSku = topProduct.ProductSku,
                    Ean = topProduct.Ean,
                    Quantity = topProduct.Quantity
                };

                topSalesProducts.Add(topSalesProduct);
            }

            return topSalesProducts.OrderByDescending(x => x.Quantity);
        }
    }
}
