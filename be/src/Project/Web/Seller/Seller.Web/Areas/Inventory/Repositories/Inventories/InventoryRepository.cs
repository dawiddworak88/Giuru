using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;
using Seller.Web.Shared.Configurations;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using System;
using Foundation.ApiExtensions.Models.Request;
using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Areas.Inventory.ApiRequestModels;
using Foundation.ApiExtensions.Models.Response;

namespace Seller.Web.Areas.Inventory.Repositories.Inventories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IApiClientService apiInventoryService;
        private readonly IOptions<AppSettings> settings;

        public InventoryRepository(
            IApiClientService apiInventoryService,
            IOptions<AppSettings> settings)
        {
            this.apiInventoryService = apiInventoryService;
            this.settings = settings;
        }

        public async Task<PagedResults<IEnumerable<InventoryItem>>> GetInventoryProductsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var productsRequestModel = new PagedRequestModelBase
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = productsRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.InventoryUrl}{ApiConstants.Inventory.InventoryApiEndpoint}"
            };

            var response = await this.apiInventoryService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<InventoryItem>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<InventoryItem>>(response.Data.Total, response.Data.PageSize)
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

        public async Task<InventoryItem> GetInventoryProductAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.InventoryUrl}{ApiConstants.Inventory.InventoryApiEndpoint}/{id}"
            };

            var response = await this.apiInventoryService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, InventoryItem>(apiRequest);
            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                return response.Data;
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
                EndpointAddress = $"{this.settings.Value.InventoryUrl}{ApiConstants.Inventory.InventoryApiEndpoint}/{id}"
            };

            var response = await this.apiInventoryService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);
            if (!response.IsSuccessStatusCode && response?.Data != null)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }
        }

        public async Task<Guid> SaveAsync(string token, string language, Guid? id, Guid? WarehouseId, Guid? ProductId, string ProductName, string ProductSku, double Quantity, string ean, int? RestockableInDays, double? AvailableQuantity, DateTime? ExpectedDelivery, Guid? OrganisationId)
        {
            var requestModel = new SaveInventoryRequestModel
            {
                Id = id,
                WarehouseId = WarehouseId,
                ProductId = ProductId,
                ProductName = ProductName,
                ProductSku = ProductSku,
                Quantity = Quantity,
                Ean = ean,
                RestockableInDays = RestockableInDays,
                AvailableQuantity = AvailableQuantity,
                ExpectedDelivery = ExpectedDelivery,
                OrganisationId = OrganisationId
            };
            var apiRequest = new ApiRequest<SaveInventoryRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.InventoryUrl}{ApiConstants.Inventory.InventoryApiEndpoint}"
            };

            var response = await this.apiInventoryService.PostAsync<ApiRequest<SaveInventoryRequestModel>, SaveInventoryRequestModel, BaseResponseModel>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Id != null)
            {
                return response.Data.Id.Value;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }
    }
}
