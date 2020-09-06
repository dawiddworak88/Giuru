using Buyer.Web.Areas.Products.ApiRequestModels;
using Buyer.Web.Areas.Products.ApiResponseModels;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.Products
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public ProductsRepository(IApiClientService apiClientService, IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(Guid? categoryId, string language, string searchTerm, int pageIndex, int itemsPerPage, string token)
        {
            var productsRequestModel = new ProductsRequestModel
            {
                CategoryId = categoryId,
                Language = language,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage
            };

            var apiRequest = new ApiRequest<ProductsRequestModel>
            {
                Data = this.apiClientService.InitializeRequestModelContext(productsRequestModel),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<ProductsRequestModel>, ProductsRequestModel, ProductsResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.PagedProducts?.Data != null)
            {
                var products = new List<Product>();

                foreach (var productResponse in response.Data.PagedProducts.Data)
                {
                    var product = new Product
                    {
                        Id = productResponse.Id,
                        Sku = productResponse.Sku,
                        Name = productResponse.Name
                    };

                    products.Add(product);
                }

                return new PagedResults<IEnumerable<Product>>(response.Data.PagedProducts.Total, response.Data.PagedProducts.PageSize)
                {
                    Data = products
                };
            }

            return default;
        }
    }
}
