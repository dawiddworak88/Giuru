using Buyer.Web.Areas.Products.ApiRequestModels;
using Buyer.Web.Areas.Products.ApiResponseModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Buyer.Web.Areas.Products.DomainModels;
using Foundation.Extensions.Exceptions;
using System.Net;

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
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}/{productId}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, ProductResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return MapProductResponseToProduct(response.Data);
            }

            return default;
        }

        public async Task<Product> GetProductAsync(string sku, string language, string token)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}/sku/{sku}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, ProductResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return MapProductResponseToProduct(response.Data);
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(
            IEnumerable<Guid> ids, 
            Guid? categoryId, 
            Guid? sellerId, 
            string language, 
            string searchTerm, 
            int pageIndex, 
            int itemsPerPage, 
            string token,
            string orderBy)
        {
            var productsRequestModel = new ProductsRequestModel
            {
                Ids = ids.ToEndpointParameterString(),
                CategoryId = categoryId,
                SellerId = sellerId,
                SearchTerm = searchTerm,
                HasPrimaryProduct = false,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<ProductsRequestModel>
            {
                Language = language,
                Data = productsRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<ProductsRequestModel>, ProductsRequestModel, PagedResults<IEnumerable<ProductResponseModel>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var products = new List<Product>();

                foreach (var productResponse in response.Data.Data)
                {
                    var product = MapProductResponseToProduct(productResponse);

                    products.Add(product);
                }

                return new PagedResults<IEnumerable<Product>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = products
                };
            }

            return default;
        }

        public async Task<ProductStock> GetProductStockAsync(Guid? id)
        {
            var productRequestModel = new ProductStockRequestModel
            {
                ProductId = id.Value,
            };

            var apiRequest = new ApiRequest<ProductStockRequestModel>
            {
                Data = productRequestModel,
                EndpointAddress = $"{this.settings.Value.InventoryUrl}{ApiConstants.Inventory.InventoryApiEndpoint}/product/{id}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<ProductStockRequestModel>, ProductStockRequestModel, ProductStockResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return new ProductStock
                {
                    AvailableQuantity = response.Data.AvailableQuantity,
                    RestockableInDays = response.Data.RestockableInDays,
                    ExpectedDelivery = response.Data.ExpectedDelivery
                };
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            throw new CustomException(response.Data.Message, (int)response.StatusCode);
        }

        public async Task<IEnumerable<string>> GetProductSuggestionsAsync(string searchTerm, int size, string language, string token)
        {
            var productRequestModel = new ProductSuggestionsRequestModel
            {
                SearchTerm = searchTerm,
                Size = size
            };

            var apiRequest = new ApiRequest<ProductSuggestionsRequestModel>
            {
                Language = language,
                Data = productRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductSuggestionsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<ProductSuggestionsRequestModel>, ProductSuggestionsRequestModel, IEnumerable<string>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data;
            }

            return Enumerable.Empty<string>();
        }

        private static Product MapProductResponseToProduct(ProductResponseModel productResponse)
        { 
            return new Product
            {
                Id = productResponse.Id.Value,
                PrimaryProductId = productResponse.PrimaryProductId,
                Sku = productResponse.Sku,
                Name = productResponse.Name,
                Description = productResponse.Description,
                IsNew = productResponse.IsNew,
                IsProtected = productResponse.IsProtected,
                FormData = productResponse.FormData,
                SellerId = productResponse.SellerId,
                BrandName = productResponse.BrandName,
                CategoryId = productResponse.CategoryId,
                CategoryName = productResponse.CategoryName,
                ProductVariants = productResponse.ProductVariants,
                Images = productResponse.Images,
                Files = productResponse.Files,
                Videos = productResponse.Videos,
                ProductAttributes = productResponse.ProductAttributes?.Select(x => new ProductAttribute 
                { 
                    Name = x.Name,
                    Values = x.Values.OrEmptyIfNull().Select(y => y)
                })
            };
        }
    }
}
