using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Products.ApiRequestModels;
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

        public CategoriesRepository(IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<PagedResults<IEnumerable<Category>>> GetCategoriesAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage)
        {
            var categoriesRequestModel = new CategoriesRequestModel
            {
                Language = language,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage
            };

            var apiRequest = new ApiRequest<CategoriesRequestModel>
            {
                Data = this.apiClientService.InitializeRequestModelContext(categoriesRequestModel),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.CategoriesApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<CategoriesRequestModel>, CategoriesRequestModel, PagedResults<IEnumerable<CategoryResponseModel>>>(apiRequest);

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
