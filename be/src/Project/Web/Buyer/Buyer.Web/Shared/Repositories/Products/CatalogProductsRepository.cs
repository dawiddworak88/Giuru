using Buyer.Web.Shared.ApiRequestModels.Catalogs;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.DomainModels.CatalogProducts;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Products
{
    public class CatalogProductsRepository : ICatalogProductsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public CatalogProductsRepository(IApiClientService apiClientService, IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
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
                EndpointAddress = $"{this.settings.Value.CatalogUrl}{ApiConstants.Catalog.ProductsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedCatalogProductsRequestModel>, PagedCatalogProductsRequestModel, PagedResults<IEnumerable<CatalogProduct>>>(apiRequest);

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
    }
}
