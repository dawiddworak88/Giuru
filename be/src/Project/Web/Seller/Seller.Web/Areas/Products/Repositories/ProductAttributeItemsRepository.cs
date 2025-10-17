using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Products.ApiRequestModels;
using Seller.Web.Areas.Products.Repositories;
using Seller.Web.Shared.ApiRequestModels;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.DomainModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.ProductAttributes.Repositories
{
    public class ProductAttributeItemsRepository : IProductAttributeItemsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public ProductAttributeItemsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<Guid> SaveAsync(
            string token,
            string language,
            Guid? id,
            Guid? productAttributeId,
            string name)
        {
            var requestModel = new SaveProductAttributeItemRequestModel
            {
                Id = id,
                ProductAttributeId = productAttributeId,
                Name = name
            };

            var apiRequest = new ApiRequest<SaveProductAttributeItemRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductAttributeItemsApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<SaveProductAttributeItemRequestModel>, SaveProductAttributeItemRequestModel, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Id != null)
            {
                return response.Data.Id.Value;
            }

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task DeleteAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductAttributeItemsApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }

        public async Task<ProductAttributeItem> GetByIdAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductAttributeItemsApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, ProductAttributeItem>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data;
            }

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<ProductAttributeItem>>> GetAsync(string token, string language, Guid? productAttributeId, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var productAttributeItems = new PagedProductAttributeItemsRequestModel
            {
                ProductAttributeId = productAttributeId,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<PagedProductAttributeItemsRequestModel>
            {
                Language = language,
                Data = productAttributeItems,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductAttributeItemsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedProductAttributeItemsRequestModel>, PagedProductAttributeItemsRequestModel, PagedResults<IEnumerable<ProductAttributeItem>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<ProductAttributeItem>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = response.Data.Data
                };
            }

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<IEnumerable<ProductAttributeItem>> GetAsync(string token, string language, Guid? productAttributeId)
        {
            var requestModel = new PagedProductAttributeItemsRequestModel
            {
                ProductAttributeId = productAttributeId,
                PageIndex = PaginationConstants.DefaultPageIndex,
                ItemsPerPage = PaginationConstants.DefaultPageSize
            };

            var apiRequest = new ApiRequest<PagedProductAttributeItemsRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductAttributeItemsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedProductAttributeItemsRequestModel>, PagedProductAttributeItemsRequestModel, PagedResults<IEnumerable<ProductAttributeItem>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var productAttributeItems = new List<ProductAttributeItem>();

                productAttributeItems.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await this.apiClientService.GetAsync<ApiRequest<PagedProductAttributeItemsRequestModel>, PagedProductAttributeItemsRequestModel, PagedResults<IEnumerable<ProductAttributeItem>>>(apiRequest);

                    if (nextPagesResponse.IsSuccessStatusCode is false)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Any())
                    {
                        productAttributeItems.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return productAttributeItems;
            }

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }
    }
}
