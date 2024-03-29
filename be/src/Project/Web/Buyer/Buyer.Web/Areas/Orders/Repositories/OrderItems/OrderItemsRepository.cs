using Buyer.Web.Areas.Orders.DomainModels;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.Extensions.Options;
using Buyer.Web.Shared.Configurations;
using System;
using System.Threading.Tasks;
using Foundation.Extensions.Exceptions;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Shared.Definitions;
using Buyer.Web.Areas.Orders.ApiRequestModels;
using Foundation.ApiExtensions.Models.Response;

namespace Buyer.Web.Areas.Orders.Repositories.OrderItems
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;

        public async Task<OrderItem> GetAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.OrderItemsApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, OrderItem>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data;
            }

            return default;
        }

        public async Task<OrderItemStatusChanges> GetStatusChangesAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.OrderItemStatusChangesApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, OrderItemStatusChanges>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data;
            }

            return default;
        }

        public async Task UpdateStatusAsync(string token, string language, Guid? id, Guid? orderItemStatusId)
        {
            var requestModel = new UpdateOrderItemStatusApiRequestModel
            {
                Id = id,
                OrderItemStatusId = orderItemStatusId,
            };

            var apiRequest = new ApiRequest<UpdateOrderItemStatusApiRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.UpdateOrderItemStatusApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<UpdateOrderItemStatusApiRequestModel>, UpdateOrderItemStatusApiRequestModel, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }
    }
}
