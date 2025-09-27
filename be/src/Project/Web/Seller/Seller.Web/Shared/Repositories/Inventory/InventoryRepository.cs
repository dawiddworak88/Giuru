using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Microsoft.Extensions.Options;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.DomainModels.Inventory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Shared.Repositories.Inventory
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

        public async Task<IEnumerable<InventoryItem>> GetAvailbleProductsByProductIdsAsync(string token, string language, IEnumerable<Guid> ids)
        {
            var requestModel = new PagedRequestModelBase
            {
                Ids = ids.ToEndpointParameterString()
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{_settings.Value.InventoryUrl}{ApiConstants.Inventory.InventoryProductsApiEndpoint}"
            };

            var response = await _apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, IEnumerable<InventoryItem>>(apiRequest);

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data is not null)
            {
                return response.Data;
            }

            return default;
        }
    }
}
