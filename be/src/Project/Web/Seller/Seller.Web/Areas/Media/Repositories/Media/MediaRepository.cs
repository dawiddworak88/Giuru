using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Media.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.Repositories.Media
{
    public class MediaRepository : IMediaRepository
    {
        private readonly IApiClientService apiService;
        private readonly IOptions<AppSettings> settings;

        public MediaRepository(
            IApiClientService apiService,
            IOptions<AppSettings> settings)
        {
            this.apiService = apiService;
            this.settings = settings;
        }

        public async Task DeleteAsync(string token, string language, Guid? mediaId)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.MediaUrl}{ApiConstants.Media.FilesApiEndpoint}/{mediaId}"
            };

            var response = await this.apiService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode && response?.Data != null)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }
        }

        public async Task<PagedResults<IEnumerable<MediaItem>>> GetMediaItemsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
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
                EndpointAddress = $"{this.settings.Value.MediaUrl}{ApiConstants.Media.MediaItemsApiEndpoint}"
            };

            var response = await this.apiService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<MediaItem>>>(apiRequest);
            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                return new PagedResults<IEnumerable<MediaItem>>(response.Data.Total, response.Data.PageSize)
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
