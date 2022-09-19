using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.Products.ApiRequestModels;
using Seller.Web.Areas.Products.DomainModels;
using Seller.Web.Shared.ApiRequestModels;
using Seller.Web.Shared.ApiResponseModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.Shared.Repositories.Media
{
    public class MediaItemsRepository : IMediaItemsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public MediaItemsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<IEnumerable<MediaItem>> GetAllMediaItemsAsync(string token, string language, string mediaItemIds, int pageIndex, int itemsPerPage)
        {
            var mediaItemsRequestModel = new PagedMediaItemsRequestModel
            {
                Ids = mediaItemIds,
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage
            };

            var apiRequest = new ApiRequest<PagedMediaItemsRequestModel>
            {
                Language = language,
                Data = mediaItemsRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.MediaUrl}{ApiConstants.Media.MediaItemsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedMediaItemsRequestModel>, PagedMediaItemsRequestModel, PagedResults<IEnumerable<MediaItem>>>(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var mediaItems = new List<MediaItem>();

                mediaItems.AddRange(response.Data.Data);

                int totalPages = (int)Math.Ceiling(response.Data.Total / (double)itemsPerPage);

                for (int i = PaginationConstants.SecondPage; i <= totalPages; i++)
                {
                    apiRequest.Data.PageIndex = i;

                    var nextPagesResponse = await this.apiClientService.GetAsync<ApiRequest<PagedMediaItemsRequestModel>, PagedMediaItemsRequestModel, PagedResults<IEnumerable<MediaItem>>>(apiRequest);

                    if (!nextPagesResponse.IsSuccessStatusCode)
                    {
                        throw new CustomException(response.Message, (int)response.StatusCode);
                    }

                    if (nextPagesResponse.IsSuccessStatusCode && nextPagesResponse.Data?.Data != null && nextPagesResponse.Data.Data.Count() > 0)
                    {
                        mediaItems.AddRange(nextPagesResponse.Data.Data);
                    }
                }

                return mediaItems;
            }

            return default;
        }

        public async Task<MediaItem> GetMediaItemAsync(string token, string language, Guid id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.MediaUrl}{ApiConstants.Media.MediaItemsApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, MediaItem>(apiRequest);

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

        public async Task<PagedResults<IEnumerable<MediaItem>>> GetMediaItemsAsync(IEnumerable<Guid> ids, string language, int pageIndex, int itemsPerPage, string token)
        {
            var filesRequestModel = new FilesRequestModel
            {
                Ids = ids.ToEndpointParameterString(),
                PageIndex = pageIndex,
                ItemsPerPage = itemsPerPage
            };

            var apiRequest = new ApiRequest<FilesRequestModel>
            {
                Language = language,
                Data = filesRequestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.MediaUrl}{ApiConstants.Media.MediaItemsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<FilesRequestModel>, FilesRequestModel, PagedResults<IEnumerable<FileResponseModel>>>(apiRequest);

            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var mediaItems = new List<MediaItem>();

                foreach (var mediaItemResponse in response.Data.Data)
                {
                    var mediaItem = new MediaItem
                    {
                        Id = mediaItemResponse.Id.Value,
                        Name = mediaItemResponse.Name,
                        Filename = mediaItemResponse.Filename,
                        IsProtected = mediaItemResponse.IsProtected,
                        Size = mediaItemResponse.Size,
                        Description = mediaItemResponse.Description,
                        LastModifiedDate = mediaItemResponse.LastModifiedDate,
                        CreatedDate = mediaItemResponse.CreatedDate
                    };

                    mediaItems.Add(mediaItem);
                }

                return new PagedResults<IEnumerable<MediaItem>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = mediaItems
                };
            }

            return default;
        }
    }
}
