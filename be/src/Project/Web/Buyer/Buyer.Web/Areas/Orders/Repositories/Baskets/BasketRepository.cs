using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.Clients;
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
                    UnitPrice = x.UnitPrice,
                    Price = x.Price,
                    Currency = x.Currency,
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
                        UnitPrice = x.UnitPrice,
                        Price = x.Price,
                        Currency = x.Currency,
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
            string clientEmail,
            Guid? basketId,
            ClientAddress billingAddress,
            ClientAddress shippingAddress,
            string moreInfo, 
            bool hasCustomOrder,
            bool hasApprovalToSendEmail,
            IEnumerable<Guid> attachments)
        {
            var requestModel = new CheckoutBasketApiRequestModel
            {
                ClientId = clientId,
                ClientName = clientName,
                ClientEmail = clientEmail,
                BasketId = basketId,
                MoreInfo = moreInfo,
                HasCustomOrder = hasCustomOrder,
                HasApprovalToSendEmail = hasApprovalToSendEmail,
                Attachments = attachments
            };

            if (billingAddress is not null)
            {
                requestModel.BillingAddressId = billingAddress.Id;
                requestModel.BillingCompany = billingAddress.Company;
                requestModel.BillingFirstName = billingAddress.FirstName;
                requestModel.BillingLastName = billingAddress.LastName;
                requestModel.BillingRegion = billingAddress.Region;
                requestModel.BillingPostCode = billingAddress.PostCode;
                requestModel.BillingCity = billingAddress.City;
                requestModel.BillingStreet = billingAddress.Street;
                requestModel.BillingPhoneNumber = billingAddress.PhoneNumber;
                requestModel.BillingCountryId = billingAddress.CountryId;
            }

            if (shippingAddress is not null)
            {
                requestModel.ShippingAddressId = shippingAddress.Id;
                requestModel.ShippingCompany = shippingAddress.Company;
                requestModel.ShippingFirstName = shippingAddress.FirstName;
                requestModel.ShippingLastName = shippingAddress.LastName;
                requestModel.ShippingRegion = shippingAddress.Region;
                requestModel.ShippingPostCode = shippingAddress.PostCode;
                requestModel.ShippingCity = shippingAddress.City;
                requestModel.ShippingStreet = shippingAddress.Street;
                requestModel.ShippingPhoneNumber = shippingAddress.PhoneNumber;
                requestModel.ShippingCountryId = shippingAddress.CountryId;
            }

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
