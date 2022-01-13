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
using Buyer.Web.Areas.Orders.ApiRequestModels;

namespace Buyer.Web.Shared.Repositories.Products.Suggestions
{
    public class ProductSuggestionRepository : IProductSuggestionRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public ProductSuggestionRepository(
            IApiClientService apiClientService, 
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<PagedResults<IEnumerable<Product>>> GetProductsAsync(
                    string token,
                    string language,
                    string searchTerm,
                    bool? hasPrimaryProduct,
                    Guid? sellerId,
                    int pageIndex,
                    int itemsPerPage,
                    string orderBy)
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
    }
}
