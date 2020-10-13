using Buyer.Web.Areas.Products.ApiRequestModels;
using Buyer.Web.Areas.Products.ApiResponseModels;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.Extensions.ExtensionMethods;
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

        public async Task<Product> GetProductAsync(Guid? productId, string language, string token)
        {
            var productRequestModel = new RequestModelBase
            { 
                Id = productId,
                Language = language
            };

            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = this.apiClientService.InitializeRequestModelContext(productRequestModel),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, ProductResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return this.MapProductResponseToProduct(response.Data);
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(IEnumerable<Guid> ids, Guid? categoryId, Guid? brandId, string language, string searchTerm, int pageIndex, int itemsPerPage, string token)
        {
            var productsRequestModel = new ProductsRequestModel
            {
                Ids = ids.ToEndpointParameterString(),
                CategoryId = categoryId,
                BrandId = brandId,
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

            var response = await this.apiClientService.GetAsync<ApiRequest<ProductsRequestModel>, ProductsRequestModel, PagedResults<IEnumerable<ProductResponseModel>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var products = new List<Product>();

                foreach (var productResponse in response.Data.Data)
                {
                    var product = this.MapProductResponseToProduct(productResponse);

                    products.Add(product);
                }

                return new PagedResults<IEnumerable<Product>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = products
                };
            }

            return default;
        }

        private Product MapProductResponseToProduct(ProductResponseModel productResponse)
        { 
            return new Product
            {
                Id = productResponse.Id,
                Sku = productResponse.Sku,
                Name = productResponse.Name,
                Description = productResponse.Description,
                IsNew = productResponse.IsNew,
                IsProtected = productResponse.IsProtected,
                FormData = productResponse.FormData,
                BrandId = productResponse.BrandId,
                BrandName = productResponse.BrandName,
                CategoryId = productResponse.CategoryId,
                CategoryName = productResponse.CategoryName,
                ProductVariants = productResponse.ProductVariants,
                Images = productResponse.Images,
                Files = productResponse.Files,
                Videos = productResponse.Videos
            };
        }
    }
}
