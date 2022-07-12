using Buyer.Web.Areas.DownloadCenter.DomainModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.DownloadCenter.Repositories
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

        public async Task<PagedResults<IEnumerable<DomainModels.Download>>> GetAsync(string token, string language, int pageIndex, int itemsPerPage, string searchTerm, string orderBy)
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

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<DomainModels.Download>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<DomainModels.Download>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = response.Data?.Data
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<DownloadCategory2> GetAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.DownloadUrl}{ApiConstants.DownloadCenter.DownloadCenterCategoryApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, DownloadCategory2>(apiRequest);

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
