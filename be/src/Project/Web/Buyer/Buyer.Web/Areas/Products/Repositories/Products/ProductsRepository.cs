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
using Buyer.Web.Areas.Orders.ApiRequestModels;
using Buyer.Web.Shared.ApiRequestModels.Catalogs;
using Foundation.GenericRepository.Definitions;
using Foundation.Search.Paginations;
using Foundation.Search.Models;
using Nest;
using Newtonsoft.Json;

namespace Buyer.Web.Areas.Products.Repositories.Products
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public ProductsRepository(IApiClientService apiClientService, IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<Product> GetProductAsync(Guid? productId, string language, string token)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}/{productId}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, ProductResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data is not null)
            {
                return MapProductResponseToProduct(response.Data);
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(
            string token, string language, string searchTerm, bool? hasPrimaryProduct, Guid? sellerId, int pageIndex, int itemsPerPage, string orderBy)
        {
            var productsRequestModel = new PagedProductsRequestModel
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                SellerId = sellerId,
                HasPrimaryProduct = hasPrimaryProduct,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<PagedProductsRequestModel>
            {
                Language = language,
                Data = productsRequestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedProductsRequestModel>, PagedProductsRequestModel, PagedResults<IEnumerable<Product>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data is not null)
            {
                return new PagedResults<IEnumerable<Product>>(response.Data.Total, response.Data.PageSize)
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

        public async Task<Product> GetProductAsync(string sku, string language, string token)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}/sku/{sku}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, ProductResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data is not null)
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
            bool? hasPrimaryProduct,
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
                HasPrimaryProduct = hasPrimaryProduct,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<ProductsRequestModel>
            {
                Language = language,
                Data = productsRequestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<ProductsRequestModel>, ProductsRequestModel, PagedResults<IEnumerable<ProductResponseModel>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data is not null)
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
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = new RequestModelBase(),
                EndpointAddress = $"{_settings.Value.InventoryUrl}{ApiConstants.Inventory.InventoryApiEndpoint}/product/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, ProductStockResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data is not null)
            {
                return new ProductStock
                {
                    AvailableQuantity = response.Data.AvailableQuantity,
                    RestockableInDays = response.Data.RestockableInDays,
                    ExpectedDelivery = response.Data.ExpectedDelivery
                };
            }

            return default;
        }

        public async Task<ProductStock> GetProductOutletAsync(Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = new RequestModelBase(),
                EndpointAddress = $"{_settings.Value.InventoryUrl}{ApiConstants.Outlet.ProductOutletApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, ProductStockResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data is not null)
            {
                return new ProductStock
                {
                    AvailableQuantity = response.Data.AvailableQuantity,
                    RestockableInDays = response.Data.RestockableInDays,
                    ExpectedDelivery = response.Data.ExpectedDelivery,
                    Title = response.Data.Title
                };
            }

            return default;
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
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductSuggestionsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<ProductSuggestionsRequestModel>, ProductSuggestionsRequestModel, IEnumerable<string>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data is not null)
            {
                return response.Data;
            }

            return Enumerable.Empty<string>();
        }

        public async Task<PagedResults<IEnumerable<ProductFile>>> GetProductFilesAsync(string token, string language, Guid? id, int pageIndex, int itemsPerPage, string searchTerm, string orderBy)
        {
            var requestModel = new PagedRequestModelBase
            {
                Id = id,
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
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductFilesApiEndpoint}/{id}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<ProductFile>>>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data?.Data is not null)
            {
                return new PagedResults<IEnumerable<ProductFile>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = response.Data.Data
                };
            }

            return default;
        }
        
        public async Task<IEnumerable<Product>> GetProductsBySkusAsync(string token, string language, IEnumerable<string> skus)
        {
            var requestModel = new GetProductsBySkusRequestModel
            {
                Skus = skus.ToEndpointParameterString(),
                PageIndex = Constants.DefaultPageIndex,
                ItemsPerPage = Constants.DefaultItemsPerPage
            };

            var apiRequest = new ApiRequest<GetProductsBySkusRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsSkusApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<GetProductsBySkusRequestModel>, GetProductsBySkusRequestModel, PagedResults<IEnumerable<Product>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var products = new List<Product>();

                products.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)Constants.DefaultItemsPerPage);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await _apiClientService.GetAsync<ApiRequest<GetProductsBySkusRequestModel>, GetProductsBySkusRequestModel, PagedResults<IEnumerable<Product>>>(apiRequest);

                    if (nextPagesResponse.IsSuccessStatusCode is false)
                    {
                        throw new CustomException(nextPagesResponse.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Any())
                    {
                        products.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return products;
            }

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;            
        }

        private static Product MapProductResponseToProduct(ProductResponseModel productResponse)
        { 
            if (productResponse is not null)
            {
                return new Product
                {
                    Id = productResponse.Id.Value,
                    PrimaryProductId = productResponse.PrimaryProductId,
                    PrimaryProductSku = productResponse.PrimaryProductSku,
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
                    Ean = productResponse.Ean,
                    ProductVariants = productResponse.ProductVariants,
                    Images = productResponse.Images,
                    Files = productResponse.Files,
                    Videos = productResponse.Videos,
                    ProductAttributes = productResponse.ProductAttributes?.Select(x => new ProductAttribute
                    {
                        Key = x.Key,
                        Name = x.Name,
                        Values = x.Values.OrEmptyIfNull().Select(y => y)
                    })
                };
            }

            return default;
        }

        public async Task<PagedResultsWithFilters<IEnumerable<Product>>> GetProductsWithFiltersAsync(
            string token, 
            string language,
            IEnumerable<Guid> ids,
            string searchTerm, 
            int pageIndex, 
            int itemsPerPage, 
            string source, 
            string orderBy,
            QueryFilters filters)
        {
            var requestQuery = new ProductsFiltersRequestModel
            {
                Ids = ids.ToEndpointParameterString(),
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                Source = source,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<QueryFilters>
            {
                Language = language,
                Data = filters,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsSearchApiEndpoint}"
            };

            var response = await _apiClientService.PostWithQueryAsync<ApiRequest<QueryFilters>, QueryFilters, ProductsFiltersRequestModel, PagedResultsWithFilters<IEnumerable<ProductResponseModel>>>(apiRequest, requestQuery);

            if (response.IsSuccessStatusCode && response.Data?.Data is not null)
            {
                var products = new List<Product>();

                foreach (var productResponse in response.Data.Data)
                {
                    var product = MapProductResponseToProduct(productResponse);

                    products.Add(product);
                }

                return new PagedResultsWithFilters<IEnumerable<Product>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = products,
                    Filters = response.Data.Filters
                };
            }

            return default;
        }
    }
}
