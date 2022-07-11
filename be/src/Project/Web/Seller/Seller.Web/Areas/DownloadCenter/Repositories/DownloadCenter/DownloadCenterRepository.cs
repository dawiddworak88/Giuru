using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.DownloadCenter.ApiRequestModels;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.DownloadCenter.Repositories.DownloadCenter
{
    public class DownloadCenterRepository : IDownloadCenterRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public DownloadCenterRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task DeleteAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.DownloadUrl}{ApiConstants.DownloadCenter.DownloadCenterApiEndponint}/{id}"
            };

            var response = await this.apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode && response?.Data != null)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }
        }

        public async Task<DownloadCenterItem> GetAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.DownloadUrl}{ApiConstants.DownloadCenter.DownloadCenterApiEndponint}/{id}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, DownloadCenterItem>(apiRequest);

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

        public async Task<PagedResults<IEnumerable<DownloadCenterItem>>> GetDownloadCenterAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
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
                EndpointAddress = $"{this.settings.Value.DownloadUrl}{ApiConstants.DownloadCenter.DownloadCenterApiEndponint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<DownloadCenterItem>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<DownloadCenterItem>>(response.Data.Total, response.Data.PageSize)
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

        public async Task<Guid> SaveAsync(string token, string language, Guid? id, Guid? categoryId, int? order)
        {
            var requestModel = new DownloadCenterItemRequestModel
            {
                Id = id,
                CategoryId = categoryId,
                Order = order
            };

            var apiRequest = new ApiRequest<DownloadCenterItemRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.DownloadUrl}{ApiConstants.DownloadCenter.DownloadCenterApiEndponint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<DownloadCenterItemRequestModel>, DownloadCenterItemRequestModel, BaseResponseModel>(apiRequest);

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
