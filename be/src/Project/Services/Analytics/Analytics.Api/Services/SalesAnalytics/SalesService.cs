using Analytics.Api.Infrastructure;
using Analytics.Api.Infrastructure.Entities.SalesAnalytics;
using Analytics.Api.ServicesModels.SalesAnalytics;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Analytics.Api.Services.SalesAnalytics
{
    public class SalesService : ISalesService
    {
        private readonly AnalyticsContext _context;

        public SalesService(
            AnalyticsContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateSalesAnalyticsServiceModel model)
        {
            var productDimensions = _context.ProductDimensions.Where(x => x.IsActive);

            var clientDimensions = _context.ClientDimensions.Where(x => x.IsActive);

            var locationDimensions = _context.LocationDimensions.Where(x => x.IsActive);

            foreach (var salesAnalyticsItem in model.SalesAnalyticsItems.OrEmptyIfNull())
            {
                var timeDimension = new TimeDimension
                {
                    Hour = Convert.ToInt32(salesAnalyticsItem.CreatedDate.ToString("HH")),
                    Minute = Convert.ToInt32(salesAnalyticsItem.CreatedDate.ToString("mm")),
                    Second = Convert.ToInt32(salesAnalyticsItem.CreatedDate.ToString("ss")),
                    DayOfWeek = Convert.ToInt32(salesAnalyticsItem.CreatedDate.DayOfWeek),
                    Day = Convert.ToInt32(salesAnalyticsItem.CreatedDate.ToString("dd")),
                    Quarter = (Convert.ToInt32(salesAnalyticsItem.CreatedDate.ToString("MM")) + 2) / 3,
                    Month = Convert.ToInt32(salesAnalyticsItem.CreatedDate.ToString("MM")),
                    Year = Convert.ToInt32(salesAnalyticsItem.CreatedDate.ToString("yyyy"))
                };

                await _context.TimeDimensions.AddAsync(timeDimension.FillCommonProperties());

                var clientDimension = clientDimensions.FirstOrDefault(x => x.ClientId == salesAnalyticsItem.ClientId);

                if (clientDimension is null)
                {
                    clientDimension = new ClientDimension
                    {
                        OrganisationId = model.OrganisationId.Value,
                        ClientId = salesAnalyticsItem.ClientId.Value,
                        Email = salesAnalyticsItem.Email,
                        Name = salesAnalyticsItem.ClientName
                    };

                    await _context.ClientDimensions.AddAsync(clientDimension.FillCommonProperties());
                }

                var locationDimension = locationDimensions.FirstOrDefault(x => x.CountryId == salesAnalyticsItem.CountryId);

                if (locationDimension is null)
                {
                    locationDimension = new LocationDimension
                    {
                        CountryId = salesAnalyticsItem.CountryId.Value
                    };

                    await _context.LocationDimensions.AddAsync(locationDimension.FillCommonProperties());

                    foreach (var countryTranslation in salesAnalyticsItem.CountryTranslations.OrEmptyIfNull())
                    {
                        var locationDimensionTranslation = new LocationTranslationDimension
                        {
                            LocationDimensionId = locationDimension.Id,
                            Name = countryTranslation.Text,
                            Language = countryTranslation.Language
                        };

                        await _context.AddAsync(locationDimensionTranslation.FillCommonProperties());
                    }
                }

                foreach (var product in salesAnalyticsItem.Products.OrEmptyIfNull())
                {
                    var productDimension = productDimensions.FirstOrDefault(x => x.ProductId == product.Id);

                    if (productDimension is null)
                    {
                        productDimension = new ProductDimension
                        {
                            ProductId = product.Id.Value,
                            Sku = product.Sku,
                            Ean = product.Ean
                        };

                        await _context.ProductDimensions.AddAsync(productDimension.FillCommonProperties());

                        var productDimensionTranslation = new ProductTranslationDimension
                        {
                            ProductDimensionId = productDimension.Id,
                            Name = product.Name,
                            Language = model.Language
                        };

                        await _context.ProductTranslationDimensions.AddAsync(productDimensionTranslation.FillCommonProperties());
                    }

                    var salesFact = new SalesFact
                    {
                        ProductDimensionId = productDimension.Id,
                        ClientDimensionId = clientDimension.Id,
                        TimeDimensionId = timeDimension.Id,
                        LocationDimensionId = locationDimension.Id,
                        IsOutlet = product.IsOutlet,
                        IsStock = product.IsStock,
                        Quantity = product.Quantity
                    };

                    await _context.SalesFacts.AddAsync(salesFact.FillCommonProperties());
                }

                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AnnualSalesServiceModel>> GetAnnualSalesServiceModel(GetAnnualSalesServiceModel model)
        {
            var sales = from s in _context.SalesFacts
                          join t in _context.TimeDimensions on s.TimeDimensionId equals t.Id
                          join c in _context.ClientDimensions on s.ClientDimensionId equals c.Id
                          where s.IsActive && t.IsActive
                          group s by new { t.Year, t.Month, c.OrganisationId } into sa
                          select new
                          {
                              Year = sa.Key.Year,
                              Month = sa.Key.Month,
                              Quantity = sa.Sum(x => x.Quantity),
                              OrganisationId = sa.Key.OrganisationId
                          };

            if (model.IsSeller is false)
            {
                sales = sales.Where(x => x.OrganisationId == model.OrganisationId);
            }

            var now = DateTime.UtcNow;

            var months = Enumerable.Range(-12, 12)
                .Select(x => new    
                    {
                        Year = now.AddMonths(x+1).Year,
                        Month = now.AddMonths(x+1).Month
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
                (s, g) => new AnnualSalesServiceModel 
                    { 
                        Month = s.Month, 
                        Year = s.Year, 
                        Quantity = g.Sum(x => x.Quantity)
                    }).OrderBy(x => x.Year).ThenBy(x => x.Month);

            return annualSales;
        }

        public async Task<IEnumerable<TopSalesProductsAnalyticsServiceModel>> GetTopSalesProductsAnalyticsAsync(GetTopSalesProductsAnalyticsServiceModel model)
        {
            var products = from s in _context.SalesFacts
                              join p in _context.ProductDimensions on s.ProductDimensionId equals p.Id
                              where s.IsActive && p.IsActive
                              group s by new { p.ProductId } into gp
                              where gp.Sum(x => x.Quantity) > 0
                              select new
                              {
                                  ProductId = gp.Key.ProductId,
                                  ProductSku = _context.ProductDimensions.FirstOrDefault(x => x.Id == gp.First().ProductDimensionId).Sku,
                                  ProductName = _context.ProductTranslationDimensions.FirstOrDefault(x => x.ProductDimensionId == gp.First().ProductDimensionId).Name,
                                  Ean = _context.ProductDimensions.FirstOrDefault(x => x.Id == gp.First().ProductDimensionId).Ean,
                                  OrganisationId = _context.ClientDimensions.FirstOrDefault(x => x.Id == gp.FirstOrDefault().ClientDimensionId && x.IsActive).OrganisationId,
                                  Quantity = gp.Sum(y => y.Quantity)
                              };

            products = products.ApplySort(model.OrderBy);

            if (model.IsSeller is false)
            {
                products = products.Where(x => x.OrganisationId == model.OrganisationId);
            }

            if (model.Size.HasValue)
            {
                products = products.Take(model.Size.Value);
            }

            var topProducts = new List<TopSalesProductsAnalyticsServiceModel>();

            foreach (var product in products.OrEmptyIfNull())
            {
                var topProduct = new TopSalesProductsAnalyticsServiceModel
                {
                    ProductId = product.ProductId,
                    ProductSku = product.ProductSku,
                    ProductName = product.ProductName,
                    Ean = product.Ean,
                    Quantity = product.Quantity
                };

                topProducts.Add(topProduct);
            }

            return topProducts.OrderByDescending(x => x.Quantity);
        }
    }
}
