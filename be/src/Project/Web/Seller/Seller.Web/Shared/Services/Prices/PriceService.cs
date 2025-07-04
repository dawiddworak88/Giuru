using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.ApiRequestModels;
using Seller.Web.Shared.ApiResponseModels;
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
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;
        private readonly ILogger<PriceService> _logger;

        public PriceService(
            IApiClientService apiClientService,
            IOptions<AppSettings> options,
            ILogger<PriceService> logger)
        {
            _apiClientService = apiClientService;
            _options = options;
            _logger = logger;
        }

        public async Task<IEnumerable<Price>> GetPrices(
            string token,
            DateTime pricingDate,
            IEnumerable<PriceProduct> products,
            PriceClient client)
        {
            var priceRequests = new List<PriceRequestModel>();
            var prices = new List<Price>();

            foreach (var product in products)
            {
                if (string.IsNullOrWhiteSpace(product.PrimarySku) ||
                    string.IsNullOrWhiteSpace(product.FabricsGroup))
                {
                    prices.Add(null);
                    continue;
                }

                var priceRequest = new PriceRequestModel
                {
                    PriceDrivers = CreatePriceDrivers(product, client),
                    CurrencyThreeLetterCode = client?.CurrencyCode ?? _options.Value.DefaultCurrency,
                    PricingDate = pricingDate
                };

                priceRequests.Add(priceRequest);
            }

            var requestModel = new GetPricesRequestModel
            {
                EnvironmentId = _options.Value.GrulaEnvironmentId,
                PriceRequests = priceRequests,
            };

            var apiRequest = new ApiRequest<GetPricesRequestModel>
            {
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_options.Value.GrulaUrl}{ApiConstants.Grula.PricesApiEndpoint}"
            };

            try
            {
                var response = await _apiClientService.PostAsync<ApiRequest<GetPricesRequestModel>, GetPricesRequestModel, IEnumerable<PriceResponseModel>>(apiRequest);

                if (response.IsSuccessStatusCode && response.Data != null)
                {
                    foreach (var priceResponse in response.Data)
                    {
                        if (priceResponse is not null &&
                            priceResponse.Amount is not null)
                        {
                            var price = new Price
                            {
                                CurrentPrice = priceResponse.Amount.Amount,
                                CurrencyCode = priceResponse.Amount.CurrencyThreeLetterCode
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
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching prices from the Grula API.");

                return Enumerable.Empty<Price>();
            }

            return default;
        }

        private List<PriceDriverRequestModel> CreatePriceDrivers(PriceProduct product, PriceClient client)
        {
            var priceDrivers = new List<PriceDriverRequestModel>
            {
                new PriceDriverRequestModel
                {
                    Name = PriceDriversConstants.ProductDriver,
                    Value = product.PrimarySku
                },
                new PriceDriverRequestModel
                {
                    Name = PriceDriversConstants.FabricsGroupDriver,
                    Value = product.FabricsGroup
                }
            };

            if (!string.IsNullOrWhiteSpace(product.SleepAreaSize))
            {
                priceDrivers.Add(new PriceDriverRequestModel
                {
                    Name = PriceDriversConstants.SleepAreaDriver,
                    Value = product.SleepAreaSize
                });
            }

            if (!string.IsNullOrWhiteSpace(product.ExtraPacking))
            {
                priceDrivers.Add(new PriceDriverRequestModel
                {
                    Name = PriceDriversConstants.ProductExtraPackingDriver,
                    Value = product.ExtraPacking
                });
            }

            if (!string.IsNullOrWhiteSpace(product.PaletteSize))
            {
                priceDrivers.Add(new PriceDriverRequestModel
                {
                    Name = PriceDriversConstants.PaletteSizeDriver,
                    Value = product.PaletteSize
                });
            }

            if (!string.IsNullOrWhiteSpace(product.IsOutlet))
            {
                priceDrivers.Add(new PriceDriverRequestModel
                {
                    Name = PriceDriversConstants.OutletDriver,
                    Value = product.IsOutlet
                });
            }

            if (client is not null)
            {
                if (!string.IsNullOrWhiteSpace(client.Name))
                {
                    priceDrivers.Add(new PriceDriverRequestModel
                    {
                        Name = PriceDriversConstants.ClientDriver,
                        Value = client.Name
                    });
                }

                if (!string.IsNullOrWhiteSpace(client.ExtraPacking))
                {
                    priceDrivers.Add(new PriceDriverRequestModel
                    {
                        Name = PriceDriversConstants.ClientExtraPackingDriver,
                        Value = client.ExtraPacking
                    });
                }

                if (!string.IsNullOrWhiteSpace(client.PaletteLoading))
                {
                    priceDrivers.Add(new PriceDriverRequestModel
                    {
                        Name = PriceDriversConstants.PaletteLoadingDriver,
                        Value = client.PaletteLoading
                    });
                }

                if (!string.IsNullOrWhiteSpace(client.Country))
                {
                    priceDrivers.Add(new PriceDriverRequestModel
                    {
                        Name = PriceDriversConstants.ClientCountryDriver,
                        Value = client.Country
                    });
                }

                if (!string.IsNullOrWhiteSpace(client.DeliveryZipCode))
                {
                    priceDrivers.Add(new PriceDriverRequestModel
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
