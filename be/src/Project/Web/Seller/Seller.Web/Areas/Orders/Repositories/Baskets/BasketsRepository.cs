using Foundation.ApiExtensions.Communications;
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
    public class BasketsRepository : IBasketsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public BasketsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<Basket> SaveAsync(string token, string language, Guid? id, IEnumerable<OrderItem> items)
        {
            var requestModel = new SaveBasketRequestModel
            {
                Id = id,
                Items = items.OrEmptyIfNull().Select(x => new BasketItemRequestModel
                {
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    DeliveryFrom = x.DeliveryFrom,
                    DeliveryTo = x.DeliveryTo,
                    MoreInfo = x.MoreInfo
                })
            };

            var apiRequest = new ApiRequest<SaveBasketRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.BasketUrl}{ApiConstants.Baskets.BasketsApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<SaveBasketRequestModel>, SaveBasketRequestModel, BasketResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return new Basket
                { 
                    Id = response.Data.Id,
                    Items = response.Data.Items.OrEmptyIfNull().Select(x => new OrderItem
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        DeliveryFrom = x.DeliveryFrom,
                        DeliveryTo = x.DeliveryTo,
                        MoreInfo = x.MoreInfo
                    })
                };
            }

            throw new CustomException(response.Message, (int)response.StatusCode);
        }
    }
}
