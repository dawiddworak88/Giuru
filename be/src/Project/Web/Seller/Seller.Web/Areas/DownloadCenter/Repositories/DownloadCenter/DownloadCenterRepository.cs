using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Foundation.Media.Services.MediaServices;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.DownloadCenter.ApiRequestModels;
using Seller.Web.Areas.DownloadCenter.ApiResponseModels;
using Seller.Web.Areas.DownloadCenter.DomainModels;
using Seller.Web.Areas.Shared.Repositories.Media;
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
        private readonly IMediaItemsRepository mediaItemsRepository;
        private readonly IMediaService mediaService;

        public DownloadCenterRepository(
            IApiClientService apiClientService,
            IMediaItemsRepository mediaItemsRepository,
            IMediaService mediaService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
            this.mediaService = mediaService;
            this.mediaItemsRepository = mediaItemsRepository;
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

        public async Task<Test> GetAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.DownloadUrl}{ApiConstants.DownloadCenter.DownloadCenterApiEndponint}/{id}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, Test>(apiRequest);

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

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<DownloadCenterItemApiResponseModel>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var downloadCenterFiles = new List<DownloadCenterItem>();

                foreach(var downloadCenterFile in response.Data.Data.OrEmptyIfNull())
                {
                    var downloadCenterFileItem = new DownloadCenterItem
                    {
                        Id = downloadCenterFile.Id,
                        Categories = String.Join(", ", downloadCenterFile.Categories.OrEmptyIfNull()),
                        LastModifiedDate = downloadCenterFile.LastModifiedDate,
                        CreatedDate = downloadCenterFile.CreatedDate
                    };

                    var file = await this.mediaItemsRepository.GetMediaItemAsync(token, language, downloadCenterFile.Id);

                    if (file is not null)
                    {
                        downloadCenterFileItem.Name = file.Name;
                        downloadCenterFileItem.Url = file.MimeType.StartsWith("image") ? this.mediaService.GetMediaUrl(downloadCenterFile.Id, 200) : this.mediaService.GetNonCdnMediaUrl(downloadCenterFile.Id);
                    }

                    downloadCenterFiles.Add(downloadCenterFileItem);
                }

                return new PagedResults<IEnumerable<DownloadCenterItem>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = downloadCenterFiles
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<Guid> SaveAsync(string token, string language, Guid? id, IEnumerable<Guid> categoriesIds, IEnumerable<Guid> files)
        {
            var requestModel = new DownloadCenterItemApiRequestModel
            {
                Id = id,
                CategoriesIds = categoriesIds,
                Files = files
            };

            var apiRequest = new ApiRequest<DownloadCenterItemApiRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.DownloadUrl}{ApiConstants.DownloadCenter.DownloadCenterApiEndponint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<DownloadCenterItemApiRequestModel>, DownloadCenterItemApiRequestModel, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data?.Id != null)
            {
                return response.Data.Id.Value;
            }

            return default;
        }
    }
}
