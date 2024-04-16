using Buyer.Web.Areas.Products.ApiRequestModels;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.Inventories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IApiClientService _apiClientService;
        private readonly IOptions<AppSettings> _settings;

        public InventoryRepository(
            IApiClientService apiClientService, 
            IOptions<AppSettings> settings)
        {
            _apiClientService = apiClientService;
            _settings = settings;
        }

        public async Task<PagedResults<IEnumerable<InventorySum>>> GetAvailbleProductsInventory(
            string language,
            int pageIndex, 
            int itemsPerPage,
            string token)
        {
            var requestModel = new PagedRequestModelBase
            {
                ItemsPerPage = itemsPerPage,
                PageIndex = pageIndex
            };

            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Data = requestModel,
                Language = language,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.InventoryUrl}{ApiConstants.Inventory.AvailableProductsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, PagedResults<IEnumerable<InventorySumResponseModel>>> (apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<InventorySum>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = response.Data.Data.OrEmptyIfNull().Select(x => new InventorySum 
                    { 
                        RestockableInDays = x.RestockableInDays,
                        AvailableQuantity = x.AvailableQuantity,
                        ExpectedDelivery = x.ExpectedDelivery,
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        ProductSku = x.ProductSku,
                        Quantity = x.Quantity
                    })
                };
            }

            return default;
        }

        public async Task<IEnumerable<InventorySum>> GetAvailbleProductsInventoryByIds(string token, string language, IEnumerable<Guid> ids)
        {
            var requestModel = new PagedRequestModelBase
            {
                Ids = ids.ToEndpointParameterString(),
                PageIndex = PaginationConstants.DefaultPageIndex,
                ItemsPerPage = PaginationConstants.DefaultPageSize
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.InventoryUrl}{ApiConstants.Inventory.AvailableProductsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<InventorySum>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var availableProducts = new List<InventorySum>();

                availableProducts.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<InventorySum>>>(apiRequest);

                    if (!nextPagesResponse.IsSuccessStatusCode)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Any())
                    {
                        availableProducts.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return availableProducts;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<IEnumerable<string>> GetAvailbleProductsInventorySuggestions(string token, string language, string searchTerm, int suggestionsCount)
        {
            var requestModel = new AvaibleProductsInventorySuggesrtionsRequestModel
            {
                SearchTerm = searchTerm,
                SuggestionsCount = suggestionsCount
            };

            var apiRequest = new ApiRequest<AvaibleProductsInventorySuggesrtionsRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.InventoryUrl}{ApiConstants.Inventory.AvailableProductsSuggestionsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<AvaibleProductsInventorySuggesrtionsRequestModel>, AvaibleProductsInventorySuggesrtionsRequestModel, IEnumerable<string>>(apiRequest);

            if (response.IsSuccessStatusCode)
            {
                return response.Data;
            }

            return default;
        }
    }
}
