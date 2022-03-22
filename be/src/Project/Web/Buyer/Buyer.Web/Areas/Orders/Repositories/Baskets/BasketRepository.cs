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
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Repositories.Baskets
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public BasketRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
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
                EndpointAddress = $"{this.settings.Value.BasketUrl}{ApiConstants.Baskets.BasketsApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<SaveBasketApiRequestModel>, SaveBasketApiRequestModel, BasketApiResponseModel>(apiRequest);
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
                        DeliveryFrom = x.DeliveryFrom,
                        DeliveryTo = x.DeliveryTo,
                        MoreInfo = x.MoreInfo
                    })
                };
            }

            throw new CustomException(response.Message, (int)response.StatusCode);
        }

        public async Task CheckoutBasketAsync(string token, string language, Guid? clientId, string clientName, Guid? basketId, DateTime? expectedDelivery, string moreInfo)
        {
            var requestModel = new CheckoutBasketRequestModel
            {
                ClientId = clientId,
                ClientName = clientName,
                BasketId = basketId,
                ExpectedDeliveryDate = expectedDelivery,
                MoreInfo = moreInfo
            };

            var apiRequest = new ApiRequest<CheckoutBasketRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.BasketUrl}{ApiConstants.Baskets.BasketsCheckoutApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<CheckoutBasketRequestModel>, CheckoutBasketRequestModel, BaseResponseModel>(apiRequest);
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
                EndpointAddress = $"{this.settings.Value.BasketUrl}{ApiConstants.Baskets.BasketsApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Basket>(apiRequest);
            if (response.IsSuccessStatusCode && response.Data != null)
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
                EndpointAddress = $"{this.settings.Value.BasketUrl}{ApiConstants.Baskets.BasketsApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);
            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }
    }
}
