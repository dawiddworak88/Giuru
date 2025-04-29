using Buyer.Web.Shared.ApiRequestModels.Price;
using Buyer.Web.Shared.ApiResponseModels;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Prices;
using Buyer.Web.Shared.DomainModels.Prices;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Prices
{
    public class PriceService : IPriceService
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;

        public PriceService(
            IApiClientService apiClientService,
            IOptions<AppSettings> options)
        {
            _apiClientService = apiClientService;
            _options = options;
        }

        public async Task<Price> GetPrice(
            string token,
            string currencyCode,
            DateTime pricingDate,
            PriceProduct product)
        {
            if (product is null)
            {
                return default;
            }

            var priceDrivers = new List<PriceDriverRequestModel>();

            if (!string.IsNullOrWhiteSpace(product.PrimarySku))
            {
                var productPriceDriver = new PriceDriverRequestModel
                {
                    Name = PriceDriversConstants.ProductDriver,
                    Value = product.PrimarySku
                };

                priceDrivers.Add(productPriceDriver);
            }

            if (!string.IsNullOrWhiteSpace(product.FabricsGroup))
            {
                var fabricsPriceDriver = new PriceDriverRequestModel
                {
                    Name = PriceDriversConstants.FabricsGroupDriver,
                    Value = product.FabricsGroup
                };

                priceDrivers.Add(fabricsPriceDriver);
            }

            var requestModel = new PriceRequestModel
            {
                PriceDrivers = priceDrivers,
                CurrencyThreeLetterCode = currencyCode,
                PricingDate = pricingDate
            };

            var apiRequest = new ApiRequest<PriceRequestModel>
            {
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_options.Value.GrulaUrl}{ApiConstants.Grula.PriceApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<PriceRequestModel>, PriceRequestModel, PriceResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return new Price
                {
                    Amount = response.Data.Amount.Amount,
                    CurrencyCode = response.Data.Amount.CurrencyThreeLetterCode,
                };
            }

            return default;
        }

        public async Task<IEnumerable<Price>> GetPrices(string token, string currencyCode, DateTime pricingDate, IEnumerable<PriceProduct> products)
        {
            if (!products.Any())
            {
                return default;
            }

            var priceRequests = new List<PriceRequestModel>();

            foreach (var product in products)
            {
                var productPriceDrivers = new List<PriceDriverRequestModel>();

                if (string.IsNullOrWhiteSpace(product.PrimarySku) is false)
                {
                    var productPriceDriver = new PriceDriverRequestModel
                    {
                        Name = PriceDriversConstants.ProductDriver,
                        Value = product.PrimarySku
                    };

                    productPriceDrivers.Add(productPriceDriver);
                }

                if (string.IsNullOrWhiteSpace(product.FabricsGroup) is false)
                {
                    var fabricsPriceDriver = new PriceDriverRequestModel
                    {
                        Name = PriceDriversConstants.FabricsGroupDriver,
                        Value = product.FabricsGroup
                    };

                    productPriceDrivers.Add(fabricsPriceDriver);
                }

                var priceRequest = new PriceRequestModel
                {
                    PriceDrivers = productPriceDrivers,
                    CurrencyThreeLetterCode = currencyCode,
                    PricingDate = pricingDate
                };

                priceRequests.Add(priceRequest);
            }

            var requestModel = new PricesRequestModel
            {
                PriceRequests = priceRequests,
            };

            var apiRequest = new ApiRequest<PricesRequestModel>
            {
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_options.Value.GrulaUrl}{ApiConstants.Grula.PricesApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<PricesRequestModel>, PricesRequestModel, IEnumerable<PriceResponseModel>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                var prices = new List<Price>();

                foreach (var priceResponse in response.Data)
                {
                    if (priceResponse != null)
                    {
                        var price = new Price
                        {
                            Amount = priceResponse.Amount.Amount,
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

            return default;
        }
    }
}
