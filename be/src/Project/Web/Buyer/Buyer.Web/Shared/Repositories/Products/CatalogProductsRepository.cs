using Buyer.Web.Shared.ApiRequestModels.Catalogs;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.CatalogProducts;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Products
{
    public class CatalogProductsRepository : ICatalogProductsRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public CatalogProductsRepository(
            IApiClientService apiClientService, 
            IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<PagedResults<IEnumerable<CatalogProduct>>> GetProductsAsync(
            string token,
            string language,
            string searchTerm,
            bool? hasPrimaryProduct,
            bool? isNew,
            Guid? sellerId,
            int pageIndex,
            int itemsPerPage,
            string orderBy)
        {
            var productsRequestModel = new PagedCatalogProductsRequestModel
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                SellerId = sellerId,
                HasPrimaryProduct = hasPrimaryProduct,
                IsNew = isNew,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<PagedCatalogProductsRequestModel>
            {
                Language = language,
                Data = productsRequestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedCatalogProductsRequestModel>, PagedCatalogProductsRequestModel, PagedResults<IEnumerable<CatalogProduct>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<CatalogProduct>>(response.Data.Total, response.Data.PageSize)
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

        public async Task<IEnumerable<CatalogProduct>> GetProductsAsync(string token, string language, string skus)
        {
            var requestModel = new GetProductsBySkusRequestModel
            {
                Skus = skus,
                PageIndex = Constants.DefaultPageIndex,
                ItemsPerPage = Constants.DefaultItemsPerPage,
            };

            var apiRequest = new ApiRequest<GetProductsBySkusRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsSkusApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<GetProductsBySkusRequestModel>, GetProductsBySkusRequestModel, PagedResults<IEnumerable<CatalogProduct>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var products = new List<CatalogProduct>();

                products.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)Constants.DefaultItemsPerPage);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await _apiClientService.GetAsync<ApiRequest<GetProductsBySkusRequestModel>, GetProductsBySkusRequestModel, PagedResults<IEnumerable<CatalogProduct>>>(apiRequest);

                    if (!nextPagesResponse.IsSuccessStatusCode)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Any())
                    {
                        products.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return products;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }
    }
}
