using Buyer.Web.Areas.Orders.ApiResponseModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Areas.Products.Repositories.Products;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Repositories
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IProductsRepository productsRepository;
        private readonly IOptions<AppSettings> settings;

        public OrdersRepository(
            IApiClientService apiClientService,
            IProductsRepository productsRepository,
            IOptions<AppSettings> settings)
        {
            this.settings = settings;
            this.productsRepository = productsRepository;
            this.apiClientService = apiClientService;
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

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Order>(apiRequest);
            if (response.IsSuccessStatusCode && response.Data != null)
            {
                var orderItem = new List<OrderItem>();
                foreach (var item in response.Data.OrderItems)
                {
                    var productAttributes = this.productsRepository.GetProductAsync(item.ProductId, null, null).Result.ProductAttributes;
                    orderItem.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        ProductSku = item.ProductSku,
                        PictureUrl = item.PictureUrl,
                        Quantity = item.Quantity,
                        ExternalReference = item.ExternalReference,
                        ExpectedDeliveryFrom = item.ExpectedDeliveryFrom,
                        ProductAttributes = productAttributes,
                        ExpectedDeliveryTo = item.ExpectedDeliveryTo,
                        MoreInfo = item.MoreInfo,
                        LastModifiedDate = item.LastModifiedDate,
                        CreatedDate = item.CreatedDate,
                    });
                }

                var order = new Order
                {
                    OrderStatusId = response.Data.OrderStatusId,
                    OrderStateId = response.Data.OrderStateId,
                    ClientId = response.Data.ClientId,
                    ClientName = response.Data.ClientName,
                    OrderStatusName = response.Data.OrderStatusName,
                    OrderItems = orderItem,
                    LastModifiedDate = response.Data.LastModifiedDate,
                    CreatedDate = response.Data.CreatedDate,
                    Id = response.Data.Id,
                };

                return order;
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

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<Order>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<Order>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = response.Data.Data
                };
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

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, IEnumerable<OrderStatus>>(apiRequest);
            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data;
            }

            return default;
        }
    }
}
