using Buyer.Web.Areas.Products.ApiResponseModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Buyer.Web.Areas.Products.DomainModels;

namespace Buyer.Web.Areas.Products.Repositories.Categories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public CategoryRepository(IApiClientService apiClientService, IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<Category> GetCategoryAsync(Guid? categoryId, string token, string language)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = new RequestModelBase(),
                Language = language,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.CategoriesApiEndpoint}/{categoryId}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, CategoryResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return new Category
                { 
                    Id = response.Data.Id.Value,
                    Name = response.Data.Name
                };
            }

            return default;
        }
    }
}
