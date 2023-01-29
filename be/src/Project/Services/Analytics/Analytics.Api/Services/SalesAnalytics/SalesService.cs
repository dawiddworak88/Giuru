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

                var locationDimension = await _context.LocationDimensions.FirstOrDefaultAsync(x => x.CountryId == client.CountryId);

                if (locationDimension is null)
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
                        await this.CreateSalesFact(clientDimension.Id, productDimension.Id, timeDimension.Id, locationDimension.Id, false, false);
                    }

                    for (int i = 0; i < product.StockQuantity; i++)
                    {
                        await this.CreateSalesFact(clientDimension.Id, productDimension.Id, timeDimension.Id, locationDimension.Id, true, false);
                    }

                    for (int i = 0; i < product.OutletQuantity; i++)
                    {
                        await this.CreateSalesFact(clientDimension.Id, productDimension.Id, timeDimension.Id, locationDimension.Id, false, true);
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

        public IEnumerable<AnnualSalesServiceModel> GetAnnualSalesServiceModel(GetAnnualSalesServiceModel model)
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
    }
}
