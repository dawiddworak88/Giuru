using Analytics.Api.Infrastructure;
using Analytics.Api.Infrastructure.Entities.SalesAnalytics;
using Analytics.Api.ServicesModels.SalesAnalytics;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Analytics.Api.Repositories.Clients;
using Analytics.Api.Repositories.Global;
using Analytics.Api.Repositories.Products;

namespace Analytics.Api.Services.SalesAnalytics
{
    public class SalesService : ISalesService
    {
        private readonly AnalyticsContext _context;
        private readonly IClientRepository _clientRepository;
        private readonly ICountriesRepository _countriesRepository;
        private readonly IProductsRepository _productsRepository;

        public SalesService(
            AnalyticsContext context,
            ICountriesRepository countriesRepository,
            IProductsRepository productsRepository,
            IClientRepository clientRepository)
        {
            _context = context;
            _clientRepository = clientRepository;
            _countriesRepository = countriesRepository;
            _productsRepository = productsRepository;
        }

        public async Task CreateAsync(CreateSalesAnalyticsServiceModel model)
        {
            var client = await _clientRepository.GetAsync(model.Token, model.ClientId);

            if (client is not null)
            {
                var clientDimension = await _context.ClientDimensions.FirstOrDefaultAsync(x => x.ClientId == model.ClientId);

                if (clientDimension is null)
                {
                    clientDimension = new ClientDimension
                    {
                        ClientId = client.Id,
                        OrganisationId = client.OrganisationId,
                        Name = client.Name,
                        Email = client.Email
                    };

                    await _context.ClientDimensions.AddAsync(clientDimension.FillCommonProperties());
                } 
                else
                {
                    clientDimension.Name = client.Name;
                    clientDimension.OrganisationId = client.OrganisationId;
                }

                var locationDimension = await _context.LocationDimensions.FirstOrDefaultAsync(x => x.CountryId == client.CountryId);

                if (locationDimension is null && client.CountryId is not null)
                {
                    var country = await _countriesRepository.GetAsync(model.Token, client.CommunicationLanguage, client.CountryId);

                    if (country is not null)
                    {
                        locationDimension = new LocationDimension
                        {
                            CountryId = country.Id
                        };

                        await _context.LocationDimensions.AddAsync(locationDimension.FillCommonProperties());

                        var locationTranslationDimension = new LocationTranslationDimension
                        {
                            Name = country.Name,
                            LocationDimensionId = locationDimension.Id,
                            Language = client.CommunicationLanguage
                        };

                        await _context.LocationTranslationDimensions.AddAsync(locationTranslationDimension.FillCommonProperties());
                    }
                }

                var timeDimension = new TimeDimension
                {
                    Hour = Convert.ToInt32(model.CreatedDate.ToString("HH")),
                    Minute = Convert.ToInt32(model.CreatedDate.ToString("mm")),
                    Second = Convert.ToInt32(model.CreatedDate.ToString("ss")),
                    DayOfWeek = Convert.ToInt32(model.CreatedDate.DayOfWeek),
                    Day = Convert.ToInt32(model.CreatedDate.ToString("dd")),
                    Quarter = (Convert.ToInt32(model.CreatedDate.ToString("MM")) + 2) / 3,
                    Month = Convert.ToInt32(model.CreatedDate.ToString("MM")),
                    Year = Convert.ToInt32(model.CreatedDate.ToString("yyyy"))
                };

                await _context.TimeDimensions.AddAsync(timeDimension.FillCommonProperties());

                foreach (var product in model.Products.OrEmptyIfNull())
                {
                    var productDimension = await _context.ProductDimensions.FirstOrDefaultAsync(x => x.ProductId == product.Id);

                    if (productDimension is null)
                    {
                        var catalogProduct = await _productsRepository.GetAsync(model.Token, client.CommunicationLanguage, product.Id);

                        if (catalogProduct is not null)
                        {
                            productDimension = new ProductDimension
                            {
                                ProductId = catalogProduct.Id,
                                Sku = catalogProduct.Sku,
                                Ean = catalogProduct.Ean
                            };

                            await _context.ProductDimensions.AddAsync(productDimension.FillCommonProperties());

                            var productDimensionTranslation = new ProductTranslationDimension
                            {
                                Name = catalogProduct.Name,
                                Language = client.CommunicationLanguage,
                                ProductDimensionId = productDimension.Id
                            };

                            await _context.ProductTranslationDimensions.AddAsync(productDimensionTranslation.FillCommonProperties());
                        }
                    }

                    for (int i = 0; i < product.Quantity; i++)
                    {
                        await this.CreateSalesFact(clientDimension.Id, productDimension.Id, timeDimension.Id, locationDimension?.Id, false, false);
                    }

                    for (int i = 0; i < product.StockQuantity; i++)
                    {
                        await this.CreateSalesFact(clientDimension.Id, productDimension.Id, timeDimension.Id, locationDimension?.Id, true, false);
                    }

                    for (int i = 0; i < product.OutletQuantity; i++)
                    {
                        await this.CreateSalesFact(clientDimension.Id, productDimension.Id, timeDimension.Id, locationDimension?.Id, false, true);
                    }
                }

                await _context.SaveChangesAsync();
            }
        }

        private async Task CreateSalesFact(
            Guid clientDimensionId, Guid productDimensionId, Guid timeDimensionId, 
            Guid? locationDimensionId, bool isStock, bool isOutlet)
        {
            var salesFact = new SalesFact
            {
                ProductDimensionId = productDimensionId,
                ClientDimensionId = clientDimensionId,
                TimeDimensionId = timeDimensionId,
                LocationDimensionId = locationDimensionId,
                IsOutlet = isStock,
                IsStock = isOutlet,
                Quantity = 1
            };

            await _context.SalesFacts.AddAsync(salesFact.FillCommonProperties());
        }

        public IEnumerable<AnnualSalesServiceModel> GetAnnualSales(GetAnnualSalesServiceModel model)
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

            if (model.ToDate.HasValue is false)
            {
                model.ToDate = DateTime.UtcNow;
            }

            var difference = (((model.ToDate.Value.Year - model.FromDate.Value.Year) * 12) + model.ToDate.Value.Month - model.FromDate.Value.Month) + 1;

            var months = Enumerable.Range(-difference, difference)
                .Select(x => new    
                    {
                        Year = model.ToDate.Value.AddMonths(x+1).Year,
                        Month = model.ToDate.Value.AddMonths(x+1).Month
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

        public IEnumerable<CountrySalesServiceModel> GetCountrySales(GetCountriesSalesServiceModel model)
        {
            if (model.ToDate.HasValue is false)
            {
                model.ToDate = model.ToDate.Value;
            }

            var countriesSales = from s in _context.SalesFacts
                                 join l in _context.LocationDimensions on s.LocationDimensionId equals l.Id
                                 join t in _context.TimeDimensions on s.TimeDimensionId equals t.Id
                                 where s.IsActive && l.IsActive && s.LocationDimensionId != null && 
                                    (t.Month >= model.FromDate.Value.Month || t.Month <= model.ToDate.Value.Month) && (t.Year >= model.FromDate.Value.Year && t.Year <= model.ToDate.Value.Year)
                                 group s by new { l.CountryId } into gpl
                                 where gpl.Sum(x => x.Quantity) > 0
                                 select new 
                                 {
                                     Id = gpl.Key.CountryId,
                                     LocationDimensionId = gpl.First().LocationDimensionId,
                                     Quantity = gpl.Sum(x => x.Quantity)
                                 };

            var translations = _context.LocationTranslationDimensions.Where(x => countriesSales.Select(y => y.LocationDimensionId).Contains(x.LocationDimensionId)).ToList();

            var countriesSalesItems = new List<CountrySalesServiceModel>();

            foreach (var countrySalesItem in countriesSales.OrEmptyIfNull())
            {
                var countrySales = new CountrySalesServiceModel
                {
                    Id = countrySalesItem.Id,
                    Name = translations.FirstOrDefault(t => t.LocationDimensionId == countrySalesItem.LocationDimensionId && t.Language == model.Language)?.Name ?? translations.FirstOrDefault(t => t.LocationDimensionId == countrySalesItem.LocationDimensionId)?.Name,
                    Quantity = countrySalesItem.Quantity
                };

                countriesSalesItems.Add(countrySales);
            }

            return countriesSalesItems;
        }

        public IEnumerable<DailySalesServiceModel> GetDailySales(GetDailySalesServiceModel model)
        {
            var sales = from s in _context.SalesFacts
                        join t in _context.TimeDimensions on s.TimeDimensionId equals t.Id
                        join c in _context.ClientDimensions on s.ClientDimensionId equals c.Id
                        where s.IsActive && t.IsActive
                        group s by new { t.Year, t.Month, t.Day, t.DayOfWeek, c.OrganisationId } into sa
                        select new
                        {
                            Year = sa.Key.Year,
                            Month = sa.Key.Month,
                            Day = sa.Key.Day,
                            DayOfWeek = sa.Key.DayOfWeek,
                            Quantity = sa.Sum(x => x.Quantity),
                            OrganisationId = sa.Key.OrganisationId
                        };

            if (model.IsSeller is false)
            {
                sales = sales.Where(x => x.OrganisationId == model.OrganisationId);
            }

            if (model.ToDate.HasValue is false)
            {
                model.ToDate = model.ToDate.Value;
            }

            var difference = (int)(model.ToDate.Value - model.FromDate.Value).TotalDays + 1;

            var days = Enumerable.Range(-difference, difference)
                .Select(x => new
                {
                    Year = model.ToDate.Value.AddDays(x + 1).Year,
                    Month = model.ToDate.Value.AddDays(x + 1).Month,
                    Day = model.ToDate.Value.AddDays(x + 1).Day,
                    DayOfWeek = (int)model.ToDate.Value.AddDays(x + 1).DayOfWeek
                });

            var dailySales = days.GroupJoin(sales,
                m => new
                {
                    DayOfWeek = m.DayOfWeek,
                    Day = m.Day,
                    Month = m.Month,
                    Year = m.Year
                },
                rev => new
                {
                    DayOfWeek = rev.DayOfWeek,
                    Day = rev.Day,
                    Month = rev.Month,
                    Year = rev.Year
                },
                (s, g) => new DailySalesServiceModel
                {
                    DayOfWeek = s.DayOfWeek,
                    Day = s.Day,
                    Month = s.Month,
                    Year = s.Year,
                    Quantity = g.Sum(x => x.Quantity)
                }).OrderBy(x => x.Year).ThenBy(x => x.Month).ThenBy(x => x.Day);

            return dailySales;
        }

        public IEnumerable<TopSalesProductsAnalyticsServiceModel> GetTopProductsSales(GetTopSalesProductsAnalyticsServiceModel model)
        {
            var topSellingProducts = from s in _context.SalesFacts
                                    join p in _context.ProductDimensions on s.ProductDimensionId equals p.Id
                                    join pt in _context.ProductTranslationDimensions on p.Translations.FirstOrDefault().Id equals pt.Id
                                    where s.CreatedDate >= model.FromDate.Value && s.CreatedDate <= model.ToDate.Value && s.IsActive == true && p.IsActive == true
                                    group new { s.Quantity, p.ProductId, pt.Name, p.Sku, p.Ean } by s.ProductDimensionId into g
                                    orderby g.Sum(sp => sp.Quantity) descending
                                    select new 
                                    { 
                                        ProductId = g.Key, 
                                        g.FirstOrDefault().Sku,
                                        g.FirstOrDefault().Ean,
                                        TotalQuantity = g.Sum(sp => sp.Quantity), 
                                        ProductName = g.FirstOrDefault().Name 
                                    };

           if (model.Size.HasValue)
            {
                topSellingProducts = topSellingProducts.Take(model.Size.Value);
            }

            return topSellingProducts.Select(x => new TopSalesProductsAnalyticsServiceModel
            {
                ProductId = x.ProductId,
                Quantity = x.TotalQuantity,
                ProductName = x.ProductName,
                Ean = x.Ean,
                ProductSku = x.Sku
            });
        }

        public IEnumerable<ClientSalesServiceModel> GetTopClientsSales(GetClientsSalesServiceModel model)
        {
            var clientsSales = from s in _context.SalesFacts
                               join c in _context.ClientDimensions on s.ClientDimensionId equals c.Id
                               where s.CreatedDate >= model.FromDate.Value && s.CreatedDate <= model.ToDate.Value && s.IsActive == true && c.IsActive == true
                               group s by new { c.ClientId, c.Name } into sc
                               orderby sc.Sum(sp => sp.Quantity) descending
                               select new ClientSalesServiceModel
                               {
                                   Id = sc.Key.ClientId,
                                   Name = sc.Key.Name,
                                   Quantity = sc.Sum(x => x.Quantity)
                               };

            if (model.Size.HasValue is true)
            {
                clientsSales = clientsSales.Take(model.Size.Value);
            }

            return clientsSales;
        }
    }
}
