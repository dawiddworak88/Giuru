using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories
{
    public class OutletRepository : IOutletRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;
        public OutletRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settgins)
        {
            _apiClientService = apiClientService;
            _settings = settgins;
        }

        public async Task<PagedResults<IEnumerable<OutletSum>>> GetOutletProductsAsync(string language, int pageIndex, int itemsPerPage, string token)
        {
            var requestModel = new PagedRequestModelBase
            {
                ItemsPerPage = itemsPerPage,
                PageIndex = pageIndex
            };

            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = requestModel,
                Language = language,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.InventoryUrl}{ApiConstants.Outlet.AvailableOutletProductsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, PagedResults<IEnumerable<OutletSum>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data is not null)
            {
                return new PagedResults<IEnumerable<OutletSum>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = response.Data.Data
                };
            }

            return default;
        }

        public async Task<IEnumerable<OutletSum>> GetOutletProductsByIdsAsync(string token, string language, IEnumerable<Guid> ids)
        {
            var requestModel = new PagedRequestModelBase
            {
                Ids = ids.ToEndpointParameterString(),
                PageIndex = PaginationConstants.DefaultPageIndex,
                ItemsPerPage = PaginationConstants.DefaultPageSize
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.InventoryUrl}{ApiConstants.Outlet.AvailableOutletProductsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<OutletSum>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var availableOutletProducts = new List<OutletSum>();

                availableOutletProducts.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<OutletSum>>>(apiRequest);

                    if (!nextPagesResponse.IsSuccessStatusCode)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Any())
                    {
                        availableOutletProducts.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return availableOutletProducts;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }
    }
}
