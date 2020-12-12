using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.Configurations;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Seller.Web.Areas.Products.ApiRequestModels;
using System;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;

namespace Seller.Web.Areas.Products.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public ProductsRepository(IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(string token, string language, string searchTerm, Guid? sellerId, int pageIndex, int itemsPerPage)
        {
            var categoriesRequestModel = new PagedProductsRequestModel
            {
                Language = language,
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                SellerId = sellerId,
                IncludeProductVariants = true
            };

            var apiRequest = new ApiRequest<PagedProductsRequestModel>
            {
                Data = categoriesRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedProductsRequestModel>, PagedProductsRequestModel, PagedResults<IEnumerable<Product>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
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

        public async Task DeleteAsync(string token, string language, Guid? id)
        {
            var deleteRequestModel = new RequestModelBase
            {
                Language = language
            };

            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = deleteRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode && response?.Data != null)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }
        }
    }
}
