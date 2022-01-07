using Buyer.Web.Areas.Products.ApiRequestModels;
using Buyer.Web.Areas.Products.ApiResponseModels;
using Buyer.Web.Shared.Configurations;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Buyer.Web.Areas.Products.DomainModels;

namespace Buyer.Web.Areas.Products.Repositories.Files
{
    public class MediaItemsRepository : IMediaItemsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public MediaItemsRepository(IApiClientService apiClientService, IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
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
