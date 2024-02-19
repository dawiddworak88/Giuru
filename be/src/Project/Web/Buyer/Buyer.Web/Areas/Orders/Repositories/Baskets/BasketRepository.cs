using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Repositories.Baskets
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
                        Id = x.Id,
                        ProductId = x.ProductId,
                        ProductSku = x.ProductSku,
                        ProductName = x.ProductName,
                        PictureUrl = x.PictureUrl,
                        Quantity = x.Quantity,
                        StockQuantity = x.StockQuantity,
                        OutletQuantity = x.OutletQuantity,
                        ExternalReference = x.ExternalReference,
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
            Guid? billingAddressId,
            string billingCompany,
            string billingFirstName,
            string billingLastName,
            string billingRegion,
            string billingPostCode,
            string billingCity,
            string billingStreet,
            string billingPhoneNumber,
            Guid? billingCountryId,
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
            string moreInfo, 
            bool hasCustomOrder,
            bool hasApprovalToSendEmail,
            IEnumerable<Guid> attachments)
        {
            var requestModel = new CheckoutBasketApiRequestModel
            {
                ClientId = clientId,
                ClientName = clientName,
                BasketId = basketId,
                BillingAddressId = billingAddressId,
                BillingCompany = billingCompany,
                BillingFirstName = billingFirstName,
                BillingLastName = billingLastName,
                BillingRegion = billingRegion,
                BillingPostCode = billingPostCode,
                BillingCity = billingCity,
                BillingStreet = billingStreet,
                BillingPhoneNumber = billingPhoneNumber,
                BillingCountryId = billingCountryId,
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
                MoreInfo = moreInfo,
                HasCustomOrder = hasCustomOrder,
                HasApprovalToSendEmail = hasApprovalToSendEmail,
                Attachments = attachments
            };

            var apiRequest = new ApiRequest<CheckoutBasketApiRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.BasketUrl}{ApiConstants.Baskets.BasketsCheckoutApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<CheckoutBasketApiRequestModel>, CheckoutBasketApiRequestModel, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }

        public async Task<Basket> GetBasketById(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.BasketUrl}{ApiConstants.Baskets.BasketsApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Basket>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data is not null)
            {
                return response.Data;
            }

            return default;
        }

        public async Task DeleteAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.BasketUrl}{ApiConstants.Baskets.BasketsApiEndpoint}/{id}"
            };

            var response = await _apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);
            
            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }
    }
}
