using Buyer.Web.Areas.Outlet.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Outlet.Repositories
{
    public class OutletRepository : IOutletRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;
        public OutletRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settgins)
        {
            this.apiClientService = apiClientService;
            this.settings = settgins;
        }

        public async Task<PagedResults<IEnumerable<OutletItem>>> GetOutletProductsAsync(string language, int pageIndex, int itemsPerPage, string token)
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
                EndpointAddress = $"{this.settings.Value.InventoryUrl}{ApiConstants.Outlet.OutletApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, PagedResults<IEnumerable<OutletItem>>>(apiRequest);
            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<OutletItem>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = response.Data.Data
                };
            }

            return default;
        }
    }
}
