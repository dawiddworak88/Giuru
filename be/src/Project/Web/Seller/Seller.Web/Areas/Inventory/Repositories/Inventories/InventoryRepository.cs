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
using System.Linq;
using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Areas.Inventory.ApiRequestModels;
using Foundation.ApiExtensions.Models.Response;
using Foundation.Extensions.ExtensionMethods;

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

        public async Task<IEnumerable<InventoryItem>> GetAllProductsAsync(string token, string language)
        {
            var productsRequestModel = new PagedRequestModelBase
            {
                PageIndex = PaginationConstants.DefaultPageIndex,
                ItemsPerPage = PaginationConstants.DefaultPageSize
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
                var products = new List<InventoryItem>();

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await this.apiInventoryService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<InventoryItem>>>(apiRequest);

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

        public async Task<IEnumerable<InventoryItem>> GetAllProductsAsync(string token, string language, IEnumerable<Guid> inventoryIds)
        {
            var productsRequestModel = new PagedRequestModelBase
            {
                Ids = inventoryIds.ToEndpointParameterString(),
                PageIndex = PaginationConstants.DefaultPageIndex,
                ItemsPerPage = PaginationConstants.DefaultPageSize
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
                var products = new List<InventoryItem>();

                products.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)PaginationConstants.DefaultPageSize);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await this.apiInventoryService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<InventoryItem>>>(apiRequest);

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

        public async Task<Guid> SaveAsync(string token, string language, Guid? id, Guid? warehouseId, Guid? productId, string productName, string productSku, int quantity, int? restockableInDays, int? availableQuantity, DateTime? expectedDelivery, Guid? organisationId)
        {
            var requestModel = new SaveInventoryRequestModel
            {
                Id = id,
                WarehouseId = warehouseId,
                ProductId = productId,
                ProductName = productName,
                ProductSku = productSku,
                Quantity = quantity,
                RestockableInDays = restockableInDays,
                AvailableQuantity = availableQuantity,
                ExpectedDelivery = expectedDelivery,
                OrganisationId = organisationId
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
