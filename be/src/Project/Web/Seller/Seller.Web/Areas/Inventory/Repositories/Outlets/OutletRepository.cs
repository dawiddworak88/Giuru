﻿using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Inventory.ApiRequestModels;
using Seller.Web.Areas.Inventory.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Inventory.Repositories
{
    public class OutletRepository : IOutletRepository
    {
        private readonly IApiClientService apiService;
        private readonly IOptions<AppSettings> settings;

        public OutletRepository(
            IApiClientService apiService,
            IOptions<AppSettings> settings)
        {
            this.apiService = apiService;
            this.settings = settings;
        }

        public async Task<PagedResults<IEnumerable<OutletItem>>> GetAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
        {
            var requestModel = new PagedRequestModelBase
            {
                SearchTerm = searchTerm,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage,
                OrderBy = orderBy
            };

            var apiRequest = new ApiRequest<PagedRequestModelBase>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.InventoryUrl}{ApiConstants.Outlet.OutletApiEndpoint}"
            };

            var response = await this.apiService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<OutletItem>>>(apiRequest);
            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<OutletItem>>(response.Data.Total, response.Data.PageSize)
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
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.InventoryUrl}{ApiConstants.Outlet.OutletApiEndpoint}/{id}"
            };

            var response = await this.apiService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);
            if (!response.IsSuccessStatusCode && response?.Data != null)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }
        }

        public async Task<Guid> SaveAsync(
            string token, string language, Guid? id, Guid? warehouseId, Guid? productId, string productName, string productSku, double quantity, string title, string description, string ean, double? availableQuantity, Guid? organisationId)
        {
            var requestModel = new SaveOutletRequestModel
            {
                Id = id,
                WarehouseId = warehouseId,
                ProductId = productId,
                ProductName = productName,
                ProductSku = productSku,
                Quantity = quantity,
                Title = title,
                Description = description,
                AvailableQuantity = availableQuantity,
                OrganisationId = organisationId,
                Ean = ean
            };
            var apiRequest = new ApiRequest<SaveOutletRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.InventoryUrl}{ApiConstants.Outlet.OutletApiEndpoint}"
            };

            var response = await this.apiService.PostAsync<ApiRequest<SaveOutletRequestModel>, SaveOutletRequestModel, BaseResponseModel>(apiRequest);

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

        public async Task<OutletItem> GetOutletItemAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.InventoryUrl}{ApiConstants.Outlet.OutletApiEndpoint}/{id}"
            };

            var response = await this.apiService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, OutletItem>(apiRequest);
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
    }
}
