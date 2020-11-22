using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Products.ApiResponseModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Shared.Configurations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Categories.Repositories
{
    public class CategoriesRepository : ICategoriesRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;
        private readonly ServicesEndpointsConfiguration servicesEndpointsConfiguration;
        private readonly ILogger logger;

        public CategoriesRepository(IApiClientService apiClientService,
            IOptions<AppSettings> settings,
            IOptionsMonitor<ServicesEndpointsConfiguration> servicesEndpointsConfiguration,
            ILogger<CategoriesRepository> logger)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
            this.servicesEndpointsConfiguration = servicesEndpointsConfiguration.CurrentValue;
            this.logger = logger;
        }

        public async Task<PagedResults<IEnumerable<Category>>> GetCategoriesAsync(string token, string language, int pageIndex, int itemsPerPage)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = this.apiClientService.InitializeRequestModelContext(new RequestModelBase()),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.CategoriesApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, PagedResults<IEnumerable<CategoryResponseModel>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                var categories = new List<Category>();

                foreach (var categoryResponse in response.Data.Data)
                {
                    var category = new Category
                    {
                        Id = categoryResponse.Id,
                        Name = categoryResponse.Name
                    };

                    categories.Add(category);
                }

                return new PagedResults<IEnumerable<Category>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = categories
                };
            }

            return default;
        }
    }
}
