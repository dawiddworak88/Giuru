using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.ApiResponseModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.Configurations;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.ApiExtensions.Models.Request;

namespace Seller.Web.Areas.Products.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public ProductRepository(IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<Product> GetProductAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = this.apiClientService.InitializeRequestModelContext(new RequestModelBase()),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, ProductResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return new Product
                { 
                    Id = response.Data.Id.Value,
                    Name = response.Data.Name,
                    Sku = response.Data.Sku,
                    FormData = response.Data.FormData,
                    LastModifiedDate = response.Data.LastModifiedDate,
                    CreatedDate = response.Data.CreatedDate
                };
            }

            return default;
        }
    }
}
