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

namespace Seller.Web.Areas.Orders.Repositories.OrderAttributeOptions
{
    public class OrderAttributeOptionsRepository : IOrderAttributeOptionsRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;

        public OrderAttributeOptionsRepository(
            IApiClientService apiClientService, 
            IOptions<AppSettings> options)
        {
            _apiClientService = apiClientService;
            _options = options;
        }

        public async Task DeleteAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.OrderAttributeOptionsApiEndpoint}/{id}"
            };

            var response = await _apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode && response?.Data != null)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }
        }

        public async Task<OrderAttributeOption> GetAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.OrderAttributeOptionsApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, OrderAttributeOption>(apiRequest);

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

        public async Task<PagedResults<IEnumerable<OrderAttributeOption>>> GetAsync(string token, string language, Guid? attributeId, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var requestModel = new PagedOrderAttributeOptionsRequestModel
            {
                AttributeId = attributeId,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<PagedOrderAttributeOptionsRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.OrderAttributeOptionsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedOrderAttributeOptionsRequestModel>, PagedOrderAttributeOptionsRequestModel, PagedResults<IEnumerable<OrderAttributeOption>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<OrderAttributeOption>>(response.Data.Total, response.Data.PageSize)
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

        public async Task<Guid> SaveAsync(string token, string language, Guid? id, string name, Guid? attributeId)
        {
            var requestModel = new OrderAttributeOptionRequestModel
            {
                Id = id,
                Name = name,
                AttributeId = attributeId
            };

            var apiRequest = new ApiRequest<OrderAttributeOptionRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.OrderAttributeOptionsApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<OrderAttributeOptionRequestModel>, OrderAttributeOptionRequestModel, BaseResponseModel>(apiRequest);

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
    }
}
