using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
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
    }
}
