using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Foundation.Media.Services.FileTypeServices;
using Foundation.Media.Services.MediaServices;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Media.ApiRequestModels;
using Seller.Web.Areas.Media.ApiResponseModels;
using Seller.Web.Areas.Media.DomainModels;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Definitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Media.Repositories.Media
{
    public class MediaRepository : IMediaRepository
    {
        private readonly IApiClientService apiService;
        private readonly IOptions<AppSettings> settings;
        private readonly IMediaService mediaService;
        private readonly IFileTypeService fileTypeService;

        public MediaRepository(
            IApiClientService apiService,
            IOptions<AppSettings> settings,
            IFileTypeService fileTypeService,
            IMediaService mediaService)
        {
            this.apiService = apiService;
            this.settings = settings;
            this.mediaService = mediaService;
            this.fileTypeService = fileTypeService;
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

            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
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

            var response = await this.apiService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<MediaItemResponseModel>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var mediaItems = new List<MediaItem>();

                foreach (var mediaItem in response.Data.Data)
                {
                    var item = new MediaItem
                    {
                        Id = mediaItem.Id,
                        FileName = mediaItem.FileName,
                        Name = mediaItem.Name,
                        Url = this.fileTypeService.IsImage(mediaItem.MimeType) ? this.mediaService.GetMediaVersionUrl(mediaItem.MediaItemVersionId.Value, Constants.PreviewMaxWidth) : null,
                        LastModifiedDate = mediaItem.LastModifiedDate,
                        CreatedDate = mediaItem.CreatedDate
                    };

                    mediaItems.Add(item);
                }

                return new PagedResults<IEnumerable<MediaItem>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = mediaItems
                };
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            return default;
        }

        public async Task<MediaItemVersions> GetMediaItemVersionsAsync(Guid? mediaId, string token, string language)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.MediaUrl}{ApiConstants.Media.MediaItemsVersionsApiEndpoint}/{mediaId}"
            };

            var response = await this.apiService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, MediaItemVersionsResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode && response?.Data is not null)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data is not null)
            {
                return new MediaItemVersions
                {
                    Id = response.Data.Id.Value,
                    Name = response.Data.Name,
                    Description = response.Data.Description,
                    MetaData = response.Data.MetaData,
                    Versions = response.Data.Versions.OrEmptyIfNull().Select(x => new MediaItem
                    {
                        Id = x.Id,
                        FileName = x.FileName,
                        Url = this.fileTypeService.IsImage(x.MimeType) ? this.mediaService.GetMediaVersionUrl(x.MediaItemVersionId.Value, Constants.PreviewMaxWidth) : this.mediaService.GetMediaVersionUrl(x.MediaItemVersionId.Value),
                        MimeType = x.MimeType,
                        LastModifiedDate = x.LastModifiedDate,
                        CreatedDate = x.CreatedDate,
                    })
                };
            }

            return default;
        }

        public async Task UpdateMediaItemVersionAsync(Guid? mediaId, string name, string description, string metadata, string token, string language)
        {
            var requestModel = new UpdateMediaItemVersionRequestModel
            {
                Id = mediaId,
                Name = name,
                Description = description,
                MetaData = metadata,
            };

            var apiRequest = new ApiRequest<UpdateMediaItemVersionRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.MediaUrl}{ApiConstants.Media.MediaItemsVersionsApiEndpoint}"
            };

            var response = await this.apiService.PostAsync<ApiRequest<UpdateMediaItemVersionRequestModel>, UpdateMediaItemVersionRequestModel, BaseResponseModel>(apiRequest);

            if (!response.IsSuccessStatusCode && response?.Data != null)
            {
                throw new CustomException(response.Data.Message, (int)response.StatusCode);
            }
        }
    }
}
