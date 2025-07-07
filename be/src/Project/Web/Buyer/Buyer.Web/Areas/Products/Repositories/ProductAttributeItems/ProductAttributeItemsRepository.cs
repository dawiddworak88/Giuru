using Buyer.Web.Areas.Products.ApiRequestModels;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.ProductAttributeItems
{
    public class ProductAttributeItemsRepository : IProductAttributeItemsRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _options;

        public ProductAttributeItemsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> options)
        {
            _apiClientService = apiClientService;
            _options = options;
        }

        public async Task<IEnumerable<ProductAttributeItem>> GetAsync(string language, Guid? productAttributeId)
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
                EndpointAddress = $"{_options.Value.CatalogUrl}{ApiConstants.Catalog.ProductAttributeItemsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedProductAttributeItemsRequestModel>, PagedProductAttributeItemsRequestModel, PagedResults<IEnumerable<ProductAttributeItem>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var productAttributeItems = new List<ProductAttributeItem>();

                productAttributeItems.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await _apiClientService.GetAsync<ApiRequest<PagedProductAttributeItemsRequestModel>, PagedProductAttributeItemsRequestModel, PagedResults<IEnumerable<ProductAttributeItem>>>(apiRequest);

                    if (!nextPagesResponse.IsSuccessStatusCode)
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

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }
    }
}
