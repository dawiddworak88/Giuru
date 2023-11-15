using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.ApiResponseModels;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.Repositories.Baskets
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public BasketRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<Basket> SaveAsync(string token, string language, Guid? id, IEnumerable<BasketItem> items)
        {
            var requestModel = new SaveBasketApiRequestModel
            {
                Id = id,
                Items = items.OrEmptyIfNull().Select(x => new BasketItemApiRequestModel
                {
                    ProductId = x.ProductId,
                    ProductSku = x.ProductSku,
                    ProductName = x.ProductName,
                    PictureUrl = x.PictureUrl,
                    Quantity = x.Quantity,
                    StockQuantity = x.StockQuantity,
                    OutletQuantity = x.OutletQuantity,
                    ExternalReference = x.ExternalReference,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                })
            };

            var apiRequest = new ApiRequest<SaveBasketApiRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.BasketUrl}{ApiConstants.Baskets.BasketsApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<SaveBasketApiRequestModel>, SaveBasketApiRequestModel, BasketApiResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return new Basket
                { 
                    Id = response.Data.Id,
                    Items = response.Data.Items.OrEmptyIfNull().Select(x => new BasketItem
                    {
                        ProductId = x.ProductId,
                        ProductSku = x.ProductSku,
                        ProductName = x.ProductName,
                        PictureUrl = x.PictureUrl,
                        Quantity = x.Quantity,
                        StockQuantity = x.StockQuantity,
                        OutletQuantity = x.OutletQuantity,
                        ExternalReference = x.ExternalReference,
                        DeliveryFrom = x.DeliveryFrom,
                        DeliveryTo = x.DeliveryTo,
                        MoreInfo = x.MoreInfo
                    })
                };
            }

            throw new CustomException(response.Message, (int)response.StatusCode);
        }

        public async Task CheckoutBasketAsync(
            string token, 
            string language, 
            Guid? clientId, 
            string clientName, 
            Guid? basketId,
            Guid? shippingAddressId,
            string shippingCompany,
            string shippingFirstName,
            string shippingLastName,
            string shippingRegion,
            string shippingPostCode,
            string shippingCity,
            string shippingStreet,
            string shippingPhoneNumber,
            Guid? shippingCountryId,
            DateTime? expectedDelivery,
            string moreInfo)
        {
            var requestModel = new CheckoutBasketRequestModel
            {
                ClientId = clientId,
                ClientName = clientName,
                BasketId = basketId,
                ShippingAddressId = shippingAddressId,
                ShippingCompany = shippingCompany,
                ShippingFirstName = shippingFirstName,
                ShippingLastName = shippingLastName,
                ShippingRegion = shippingRegion,
                ShippingPostCode = shippingPostCode,
                ShippingCity = shippingCity,
                ShippingStreet = shippingStreet,
                ShippingPhoneNumber = shippingPhoneNumber,
                ShippingCountryId = shippingCountryId,
                ExpectedDeliveryDate = expectedDelivery,
                MoreInfo = moreInfo
            };

            var apiRequest = new ApiRequest<CheckoutBasketRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.BasketUrl}{ApiConstants.Baskets.BasketsCheckoutApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<CheckoutBasketRequestModel>, CheckoutBasketRequestModel, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }
    }
}
