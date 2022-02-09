using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Outlet.DomainModels;
using Seller.Web.Shared.Configurations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Outlet.Repositories
{
    public class OutletRepository : IOutletRepository
    {
        private readonly IApiClientService apiService;
        private readonly IOptions<AppSettings> settings;

        public OutletRepository(
            IApiClientService apiService,
            IOptions<AppSettings> settings)
        {
            this.apiService = apiService;
            this.settings = settings;
        }

        public async Task<PagedResults<IEnumerable<OutletItem>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
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
                EndpointAddress = $"{this.settings.Value.InventoryUrl}{ApiConstants.Outlet.OutletApiEndpoint}"
            };

            var response = await this.apiService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<OutletItem>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<OutletItem>>(response.Data.Total, response.Data.PageSize)
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
    }
}
