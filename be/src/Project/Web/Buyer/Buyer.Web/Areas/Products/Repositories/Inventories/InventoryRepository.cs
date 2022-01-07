using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Products.Repositories.Inventories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public InventoryRepository(IApiClientService apiClientService, IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
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
                EndpointAddress = $"{this.settings.Value.InventoryUrl}{ApiConstants.Inventory.AvailableProductsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, PagedResults<IEnumerable<InventorySumResponseModel>>> (apiRequest);

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
    }
}
