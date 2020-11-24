using Buyer.Web.Shared.Catalogs.Models;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Buyer.Web.Shared.Catalogs.ApiRequestModels;
using Foundation.GenericRepository.Paginations;
using System.Linq;
using System;

namespace Buyer.Web.Shared.Catalogs.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public CatalogService(IApiClientService apiClientService, IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<IEnumerable<Category>> GetCategoriesAsync(string language, int pageIndex, int itemsPerPage)
        {
            var categoriesRequestModel = new CategoriesRequestModel
            {
                Language = language,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage
            };

            var apiRequest = new ApiRequest<CategoriesRequestModel>
            {
                Data = categoriesRequestModel,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.CategoriesApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<CategoriesRequestModel>, CategoriesRequestModel, PagedResults<IEnumerable<Category>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null && response.Data.Data.Count() > 0)
            {
                var categories = new List<Category>();

                categories.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)itemsPerPage);

                for (int i = 1; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await this.apiClientService.GetAsync<ApiRequest<CategoriesRequestModel>, CategoriesRequestModel, PagedResults<IEnumerable<Category>>>(apiRequest);

                    if (nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Count() > 0)
                    {
                        categories.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return categories;
            }

            return default;
        }
    }
}
