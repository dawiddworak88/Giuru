using Foundation.ApiExtensions.Communications;
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
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Orders.Repositories.OrderAttributeValues
{
    public class OrderAttributeValuesRepository : IOrderAttributeValuesRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;

        public OrderAttributeValuesRepository(
            IApiClientService apiClientService, 
            IOptions<AppSettings> options)
        {
            _apiClientService = apiClientService;
            _options = options;
        }

        public async Task BatchAsync(string token, string language, IEnumerable<ApiOrderAttributeValue> values)
        {
            var apiRequest = new ApiRequest<IEnumerable<ApiOrderAttributeValue>>
            {
                Language = language,
                Data = values.Select(x => new ApiOrderAttributeValue
                {
                    OrderId = x.OrderId,
                    OrderItemId = x.OrderItemId,
                    AttributeId = x.AttributeId,
                    Value = x.Value
                }),
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.BatchOrderAttributeValuesApiEndpoint}"
            };

            var response = await _apiClientService.PostAsync<ApiRequest<IEnumerable<ApiOrderAttributeValue>>, IEnumerable<ApiOrderAttributeValue>, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }

        public async Task<IEnumerable<OrderAttributeValue>> GetAsync(string token, string language, Guid? orderId, Guid? orderItemId)
        {
            var requestModel = new PagedOrderAttributeValuesRequestModel
            {
                OrderId = orderId,
                OrderItemId = orderItemId,
                PageIndex = PaginationConstants.DefaultPageIndex,
                ItemsPerPage = PaginationConstants.DefaultPageSize
            };

            var apiRequest = new ApiRequest<PagedOrderAttributeValuesRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_options.Value.OrderUrl}{ApiConstants.Order.OrderAttributeValuesApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedOrderAttributeValuesRequestModel>, PagedOrderAttributeValuesRequestModel, PagedResults<IEnumerable<OrderAttributeValue>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var fieldsValues = new List<OrderAttributeValue>();

                fieldsValues.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await _apiClientService.GetAsync<ApiRequest<PagedOrderAttributeValuesRequestModel>, PagedOrderAttributeValuesRequestModel, PagedResults<IEnumerable<OrderAttributeValue>>>(apiRequest);

                    if (!nextPagesResponse.IsSuccessStatusCode)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Count() > 0)
                    {
                        fieldsValues.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return fieldsValues;
            }

            return default;
        }
    }
}
