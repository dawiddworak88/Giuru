using Grula.PricingIntelligencePlatform.Sdk;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.DomainModels.Prices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Services.Prices
{
    public class PriceService : IPriceService
    {
        private readonly GrulaApiClient _grulaApiClient;
        private readonly IOptions<AppSettings> _options;
        private readonly ILogger<PriceService> _logger;

        public PriceService(
            GrulaApiClient grulaApiClient,
            IOptions<AppSettings> options,
            ILogger<PriceService> logger)
        {
            _grulaApiClient = grulaApiClient;
            _options = options;
            _logger = logger;
        }

        public async Task<IEnumerable<Price>> GetPrices(
            string token,
            DateTime pricingDate,
            IEnumerable<PriceProduct> products,
            PriceClient client)
        {
            var priceRequests = new List<PriceRequest>();
            var prices = new List<Price>();

            foreach (var product in products)
            {
                if (string.IsNullOrWhiteSpace(product.PrimarySku) ||
                    string.IsNullOrWhiteSpace(product.FabricsGroup))
                {
                    prices.Add(null);
                    continue;
                }

                var priceRequest = new PriceRequest
                {
                    PriceDrivers = CreatePriceDrivers(product, client),
                    CurrencyThreeLetterCode = client?.CurrencyCode ?? _options.Value.DefaultCurrency,
                    PricingDate = pricingDate
                };

                priceRequests.Add(priceRequest);
            }

            var priceRequestModel = new GetPricesByPriceDriversQuery
            {
                EnvironmentId = _options.Value.GrulaEnvironmentId.Value,
                PriceRequests = priceRequests,
            };

            try
            {
                var grulaPrices = await _grulaApiClient.GetPricesByPriceDriversAsync(priceRequestModel);

                foreach (var grulaPrice in grulaPrices)
                {
                    if (grulaPrice?.Amount is not null)
                    {
                        var price = new Price
                        {
                            CurrentPrice = (decimal)grulaPrice.Amount.Amount,
                            CurrencyCode = grulaPrice.Amount.CurrencyThreeLetterCode
                        };

                        prices.Add(price);
                    }
                    else
                    {
                        prices.Add(null);
                    }
                }

                return prices;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching prices from the Grula API.");

                return Enumerable.Empty<Price>();
            }
        }

        private List<PriceDriver> CreatePriceDrivers(PriceProduct product, PriceClient client)
        {
            var priceDrivers = new List<PriceDriver>
            {
                new PriceDriver
                {
                    Name = PriceDriversConstants.ProductDriver,
                    Value = product.PrimarySku
                },
                new PriceDriver
                {
                    Name = PriceDriversConstants.FabricsGroupDriver,
                    Value = product.FabricsGroup
                }
            };

            if (!string.IsNullOrWhiteSpace(product.SleepAreaSize))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.SleepAreaDriver,
                    Value = product.SleepAreaSize
                });
            }

            if (!string.IsNullOrWhiteSpace(product.ExtraPacking))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.ProductExtraPackingDriver,
                    Value = product.ExtraPacking
                });
            }

            if (!string.IsNullOrWhiteSpace(product.PaletteSize))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.PaletteSizeDriver,
                    Value = product.PaletteSize
                });
            }

            if (!string.IsNullOrWhiteSpace(product.IsOutlet))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.OutletDriver,
                    Value = product.IsOutlet
                });
            }

            if (!string.IsNullOrWhiteSpace(product.Mirror))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.MirrorDriver,
                    Value = product.Mirror
                });
            }

            if (!string.IsNullOrWhiteSpace(product.Size))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.SizeDriver,
                    Value = product.Size
                });
            }

            if (!string.IsNullOrWhiteSpace(product.Shape))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.ShapeDriver,
                    Value = product.Shape
                });
            }

            if (!string.IsNullOrWhiteSpace(product.PointsOfLight))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.PointsOfLightDriver,
                    Value = product.PointsOfLight
                });
            }

            if (!string.IsNullOrWhiteSpace(product.LampshadeType))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.LampshadeTypeDriver,
                    Value = product.LampshadeType
                });
            }

            if (!string.IsNullOrWhiteSpace(product.LampshadeSize))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.LampshadeSizeDriver,
                    Value = product.LampshadeSize
                });
            }

            if (!string.IsNullOrWhiteSpace(product.LinearLight))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.LinearLightDriver,
                    Value = product.LinearLight
                });
            }

            if (!string.IsNullOrWhiteSpace(product.PrimaryColor))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.PrimaryColorDriver,
                    Value = product.PrimaryColor
                });
            }

            if (!string.IsNullOrWhiteSpace(product.SecondaryColor))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.SecondaryColorDriver,
                    Value = product.SecondaryColor
                });
            }

            if (!string.IsNullOrWhiteSpace(product.ShelfType))
            {
                priceDrivers.Add(new PriceDriver
                {
                    Name = PriceDriversConstants.ShelfTypeDriver,
                    Value = product.ShelfType
                });
            }

            if (client is not null)
            {
                if (!string.IsNullOrWhiteSpace(client.Name))
                {
                    priceDrivers.Add(new PriceDriver
                    {
                        Name = PriceDriversConstants.ClientDriver,
                        Value = client.Name
                    });
                }

                if (!string.IsNullOrWhiteSpace(client.ExtraPacking))
                {
                    priceDrivers.Add(new PriceDriver
                    {
                        Name = PriceDriversConstants.ClientExtraPackingDriver,
                        Value = client.ExtraPacking
                    });
                }

                if (!string.IsNullOrWhiteSpace(client.PaletteLoading))
                {
                    priceDrivers.Add(new PriceDriver
                    {
                        Name = PriceDriversConstants.PaletteLoadingDriver,
                        Value = client.PaletteLoading
                    });
                }

                if (!string.IsNullOrWhiteSpace(client.Country))
                {
                    priceDrivers.Add(new PriceDriver
                    {
                        Name = PriceDriversConstants.ClientCountryDriver,
                        Value = client.Country
                    });
                }

                if (!string.IsNullOrWhiteSpace(client.DeliveryZipCode))
                {
                    priceDrivers.Add(new PriceDriver
                    {
                        Name = PriceDriversConstants.DeliveryAddressDriver,
                        Value = client.DeliveryZipCode
                    });
                }
            }

            return priceDrivers;
        }
    }
}
