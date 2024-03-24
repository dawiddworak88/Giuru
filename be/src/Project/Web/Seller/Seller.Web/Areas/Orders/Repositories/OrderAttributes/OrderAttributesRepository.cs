using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.Configurations;
using Seller.Web.Areas.Orders.DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.Exceptions;
using Seller.Web.Areas.Orders.ApiRequestModels;
using System.Linq;

namespace Seller.Web.Areas.Orders.Repositories.OrderAttributes
{
    public class OrderAttributesRepository : IOrderAttributesRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;
        private readonly IStringLocalizer<GlobalResources> _globalLocalizer;

        public OrderAttributesRepository(
            IApiClientService apiClientService, 
            IOptions<AppSettings> options, 
            IStringLocalizer<GlobalResources> globalLocalizer)
        {
            _apiClientService = apiClientService;
            _options = options;
            _globalLocalizer = globalLocalizer;
        }

        public async Task DeleteAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.OrderAttributesApiEndpoint}/{id}"
            };

            var response = await _apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode && response?.Data != null)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }
        }

        public async Task<IEnumerable<OrderAttribute>> GetAsync(string token, string language, bool? forOrderItems)
        {
            var requestModel = new PagedOrderAttributesRequestModel
            {
                PageIndex = PaginationConstants.DefaultPageIndex,
                ItemsPerPage = PaginationConstants.DefaultPageSize,
                ForOrderItems = forOrderItems
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.OrderAttributesApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<OrderAttribute>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var orderAttributes = new List<OrderAttribute>();

                orderAttributes.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<OrderAttribute>>>(apiRequest);

                    if (!nextPagesResponse.IsSuccessStatusCode)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Count() > 0)
                    {
                        orderAttributes.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return orderAttributes;
            }

            return default;
        }

        public async Task<OrderAttribute> GetAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.OrderAttributesApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, OrderAttribute>(apiRequest);

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

        public async Task<PagedResults<IEnumerable<OrderAttribute>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var requestModel = new PagedRequestModelBase
            {
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
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.OrderAttributesApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<OrderAttributeApi>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<OrderAttribute>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = response.Data.Data.Select(x => new OrderAttribute
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Type = MapAttributeTypeToText(x.Type),
                        IsRequired = x.IsRequired,
                        IsOrderItemAttribute = x.IsOrderItemAttribute,
                        IsOrderItemAttributeText = x.IsOrderItemAttribute ? _globalLocalizer.GetString("Yes") : _globalLocalizer.GetString("No"),
                        Options = x.Options.Select(o => new OrderAttributeOptionItem
                        {
                            Name = o.Name,
                            Value = o.Value
                        }),
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate
                    })
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<Guid> SaveAsync(string token, string language, Guid? id, string name, string type, bool isOrderItemAttribute)
        {
            var requestModel = new OrderAttributeRequestModel
            {
                Id = id,
                Name = name,
                Type = type,
                IsOrderItemAttribute = isOrderItemAttribute
            };

            var apiRequest = new ApiRequest<OrderAttributeRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.OrderAttributesApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<OrderAttributeRequestModel>, OrderAttributeRequestModel, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Id != null)
            {
                return response.Data.Id.Value;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        private string MapAttributeTypeToText(string type)
        {
            var typeMappings = new Dictionary<string, string>
            {
                { "boolean", _globalLocalizer.GetString("Boolean") },
                { "text", _globalLocalizer.GetString("String") },
                { "select", _globalLocalizer.GetString("Array") },
                { "number", _globalLocalizer.GetString("Number") }
            };

            if (typeMappings.TryGetValue(type, out string mapping))
            {
                return mapping;
            }

            return string.Empty;
        }
    }
}
