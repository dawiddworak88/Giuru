using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Areas.Orders.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Orders.Repositories.OrderAttributeValues
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

        public async Task<IEnumerable<OrderAttributeValue>> GetAsync(string token, string language, Guid? orderId)
        {
            var requestModel = new PagedOrderAttributeValuesRequestModel
            {
                OrderId = orderId,
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
                var attributesValues = new List<OrderAttributeValue>();

                attributesValues.AddRange(response.Data.Data);

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
                        attributesValues.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return attributesValues;
            }

            return default;
        }
    }
}
