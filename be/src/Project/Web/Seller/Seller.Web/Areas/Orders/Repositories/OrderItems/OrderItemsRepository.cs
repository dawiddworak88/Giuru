﻿using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Shared.Configurations;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Foundation.ApiExtensions.Models.Response;

namespace Seller.Web.Areas.Orders.Repositories.OrderItems
{
    public class OrderItemsRepository : IOrderItemsRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;

        public OrderItemsRepository(
            IApiClientService apiClientService, 
            IOptions<AppSettings> options)
        {
            _apiClientService = apiClientService;
            _options = options;
        }

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

        public async Task UpdateStatusAsync(string token, string language, Guid? id, Guid? orderItemStatusId, string expectedDateOfProductOnStock)
        {
            var requestModel = new UpdateOrderItemStatusRequestModel
            {
                Id = id,
                OrderItemStatusId = orderItemStatusId,
                ExpectedDateOfProductOnStock = expectedDateOfProductOnStock
            };

            var apiRequest = new ApiRequest<UpdateOrderItemStatusRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.UpdateOrderItemStatusApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<UpdateOrderItemStatusRequestModel>, UpdateOrderItemStatusRequestModel, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }
    }
}