using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Orders.ApiRequestModels;
using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.Repositories.Orders
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IApiClientService apiOrderService;
        private readonly IOptions<AppSettings> settings;

        public OrdersRepository(
            IApiClientService apiOrderService,
            IOptions<AppSettings> settings)
        {
            this.apiOrderService = apiOrderService;
            this.settings = settings;
        }

        public async Task<Order> GetOrderAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.OrderUrl}{ApiConstants.Order.OrdersApiEndpoint}/{id}"
            };

            var response = await this.apiOrderService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Order>(apiRequest);

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

        public async Task<OrderItem> GetOrderItemAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.OrderUrl}{ApiConstants.Order.OrderItemsApiEndpoint}/{id}"
            };

            var response = await this.apiOrderService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, OrderItem>(apiRequest);

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
        
        public async Task<PagedResults<IEnumerable<OrderFile>>> GetOrderFilesAsync(string token, string language, Guid? id, int pageIndex, int itemsPerPage, string searchTerm, string orderBy)
        {
            var requestModel = new PagedRequestModelBase
            {
                Id = id,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.OrderUrl}{ApiConstants.Order.OrderFilesApiEndpoint}/{id}"
            };

            var response = await this.apiOrderService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<OrderFile>>>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<OrderFile>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = response.Data.Data
                };
            }

            return default;
        }


        public async Task<OrderItemStatusChanges> GetOrderItemStatusesAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.OrderUrl}{ApiConstants.Order.OrderItemStatusesApiEndpoint}/{id}"
            };

            var response = await this.apiOrderService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, OrderItemStatusChanges>(apiRequest);

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

        public async Task<PagedResults<IEnumerable<Order>>> GetOrdersAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var ordersRequestModel = new PagedRequestModelBase
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = ordersRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.OrderUrl}{ApiConstants.Order.OrdersApiEndpoint}"
            };

            var response = await this.apiOrderService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<Order>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<Order>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = response.Data.Data
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<IEnumerable<OrderStatus>> GetOrderStatusesAsync(string token, string language)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.OrderUrl}{ApiConstants.Order.OrderStatusesApiEndpoint}"
            };

            var response = await this.apiOrderService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, IEnumerable<OrderStatus>>(apiRequest);

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

        public async Task<Guid> SaveOrderStatusAsync(string token, string language, Guid orderId, Guid orderStatusId)
        {
            var apiRequest = new ApiRequest<UpdateOrderStatusRequestModel>
            {
                Language = language,
                Data = new UpdateOrderStatusRequestModel
                {
                    OrderId = orderId,
                    OrderStatusId = orderStatusId
                },
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.OrderUrl}{ApiConstants.Order.UpdateOrderStatusApiEndpoint}"
            };

            var response = await this.apiOrderService.PostAsync<ApiRequest<UpdateOrderStatusRequestModel>, UpdateOrderStatusRequestModel, Order>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.OrderStatusId != null)
            {
                return response.Data.OrderStatusId;
            }

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task UpdateOrderItemStatusAsync(string token, string language, Guid id, Guid orderItemStatusId, string expectedDateOfProductOnStock)
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
                EndpointAddress = $"{this.settings.Value.OrderUrl}{ApiConstants.Order.UpdateOrderItemStatusApiEndpoint}"
            };

            var response = await this.apiOrderService.PostAsync<ApiRequest<UpdateOrderItemStatusRequestModel>, UpdateOrderItemStatusRequestModel, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }
    }
}