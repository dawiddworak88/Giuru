using Buyer.Web.Areas.News.ApiResponseModels;
using Buyer.Web.Areas.News.DomainModels;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;
using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.Extensions.Services.MediaServices;
using Foundation.GenericRepository.Paginations;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.News
{
    public class NewsRepository : INewsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;
        private readonly LinkGenerator linkGenerator;
        private readonly IMediaHelperService mediaService;
        private readonly ICdnService cdnService;

        public NewsRepository(
            IApiClientService apiClientService,
            LinkGenerator linkGenerator,
            IMediaHelperService mediaService,
            ICdnService cdnService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
            this.mediaService = mediaService;
            this.linkGenerator = linkGenerator;
            this.cdnService = cdnService;
        }

        public async Task<NewsItem> GetNewsItemAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.NewsUrl}{ApiConstants.News.NewsApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, NewsItem>(apiRequest);
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

        public async Task<PagedResults<IEnumerable<NewsItem>>> GetNewsItemsAsync(string token, string language, int pageIndex, int itemsPerPage, string searchTerm, string orderBy)
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
                EndpointAddress = $"{this.settings.Value.NewsUrl}{ApiConstants.News.NewsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<NewsItemResponseModel>>>(apiRequest);
            
            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var newsItems = new List<NewsItem>();

                foreach (var newsItem in response.Data.Data)
                {
                    var item = new NewsItem
                    {
                        Id = newsItem.Id,
                        PreviewImageId = newsItem.PreviewImageId,
                        ThumbnailImageId = newsItem.ThumbnailImageId,
                        CategoryId = newsItem.CategoryId,
                        CategoryName = newsItem.CategoryName,
                        Title = newsItem.Title,
                        Description = newsItem.Description,
                        Content = newsItem.Content,
                        Files = newsItem.Files,
                        Url = this.linkGenerator.GetPathByAction("Item", "News", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name, Id = newsItem.Id }),
                        IsPublished = newsItem.IsPublished,
                        LastModifiedDate = newsItem.LastModifiedDate,
                        CreatedDate = newsItem.CreatedDate,
                    };

                    if (newsItem.ThumbnailImageId.HasValue)
                    {
                        item.ThumbImageUrl = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.settings.Value.MediaUrl, newsItem.ThumbnailImageId.Value));
                        item.ThumbImages = new List<SourceViewModel>
                        {
                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.settings.Value.MediaUrl, newsItem.ThumbnailImageId.Value, 1024, 1024, true, MediaConstants.WebpExtension)) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.settings.Value.MediaUrl, newsItem.ThumbnailImageId.Value, 352, 352, true,MediaConstants.WebpExtension)) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.settings.Value.MediaUrl, newsItem.ThumbnailImageId.Value, 608, 608, true, MediaConstants.WebpExtension)) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.settings.Value.MediaUrl, newsItem.ThumbnailImageId.Value, 768, 768, true, MediaConstants.WebpExtension)) },

                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.settings.Value.MediaUrl, newsItem.ThumbnailImageId.Value, 1024, 1024, true)) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.settings.Value.MediaUrl, newsItem.ThumbnailImageId.Value, 352, 352, true)) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.settings.Value.MediaUrl, newsItem.ThumbnailImageId.Value, 608, 608, true)) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.settings.Value.MediaUrl, newsItem.ThumbnailImageId.Value, 768, 768, true)) },
                        };
                    }

                    newsItems.Add(item);
                }

                return new PagedResults<IEnumerable<NewsItem>>(response.Data.Total, response.Data.PageSize)
                {
                    Data = newsItems
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
