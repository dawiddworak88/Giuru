using Foundation.ApiExtensions.Communications;
using Foundation.ApiExtensions.Models.Request;
using Foundation.ApiExtensions.Models.Response;
using Foundation.ApiExtensions.Services.ApiClientServices;
using Foundation.ApiExtensions.Shared.Definitions;
using Foundation.Extensions.Exceptions;
using Foundation.GenericRepository.Paginations;
using Microsoft.Extensions.Options;
using Seller.Web.Areas.News.ApiRequestModels;
using Seller.Web.Areas.News.ApiResponseModels;
using Seller.Web.Areas.News.DomainModels;
using Seller.Web.Shared.Configurations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Seller.Web.Areas.News.Repositories.News
{
    public class NewsRepository : INewsRepository
    {
        private readonly IApiClientService apiClientService;
        private readonly IOptions<AppSettings> settings;

        public NewsRepository(
            IApiClientService apiClientService,
            IOptions<AppSettings> settings)
        {
            this.apiClientService = apiClientService;
            this.settings = settings;
        }

        public async Task<NewsItem> GetAsync(string token, string language, Guid? id)
        {
            var apiRequest = new ApiRequest<RequestModelBase>
            {
                Language = language,
                Data = new RequestModelBase(),
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.NewsUrl}{ApiConstants.News.NewsApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<RequestModelBase>, RequestModelBase, NewsItemResponseModel>(apiRequest);
            if (!response.IsSuccessStatusCode)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data != null)
            {
                var newsItem = new NewsItem
                {
                    Id = response.Data.Id,
                    ThumbnailImageId = response.Data.ThumbnailImageId,
                    PreviewImageId = response.Data.PreviewImageId,
                    CategoryId = response.Data.CategoryId,
                    Name = response.Data.Title,
                    Description = response.Data.Description,
                    Content = response.Data.Content,
                    CategoryName = response.Data.CategoryName,
                    IsPublished = response.Data.IsPublished,
                    Files = response.Data.Files,
                    LastModifiedDate = response.Data.LastModifiedDate,
                    CreatedDate = response.Data.CreatedDate
                };

                return newsItem;
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<NewsItem>>> GetNewsItemsAsync(string token, string language, string searchTerm, int pageIndex, int itemsPerPage, string orderBy)
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
                EndpointAddress = $"{this.settings.Value.NewsUrl}{ApiConstants.News.NewsApiEndpoint}"
            };

            var response = await this.apiClientService.GetAsync<ApiRequest<PagedRequestModelBase>, PagedRequestModelBase, PagedResults<IEnumerable<NewsItemResponseModel>>>(apiRequest);
            if (response.IsSuccessStatusCode && response.Data?.Data != null)
            {
                var newsItems = new List<NewsItem>();
                foreach (var newsItem in response.Data.Data)
                {
                    var news = new NewsItem
                    {
                        Id = newsItem.Id,
                        ThumbnailImageId = newsItem.ThumbnailImageId,
                        PreviewImageId = newsItem.PreviewImageId,
                        CategoryId = newsItem.CategoryId,
                        Name = newsItem.Title,
                        Description = newsItem.Description,
                        Content = newsItem.Content,
                        CategoryName = newsItem.CategoryName,
                        IsPublished = newsItem.IsPublished,
                        Files = newsItem.Files,
                        LastModifiedDate = newsItem.LastModifiedDate,
                        CreatedDate = newsItem.CreatedDate
                    };

                    newsItems.Add(news);
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

        public async Task<Guid> SaveAsync(
            string token, string language, Guid? id, Guid? thumbnailImageId, Guid? categoryId, Guid? previewImageId, 
            string title, string description,  string content, bool isPublished, IEnumerable<Guid> files)
        {
            var requestModel = new NewsApiRequestModel
            {
                Id = id,
                ThumbnailImageId = thumbnailImageId,
                CategoryId = categoryId,
                PreviewImageId = previewImageId,
                Title = title,
                Description = description,
                Content = content,
                IsPublished = isPublished,
                Files = files
            };

            var apiRequest = new ApiRequest<NewsApiRequestModel>
            {
                Language = language,
                Data = requestModel,
                AccessToken = token,
                EndpointAddress = $"{this.settings.Value.NewsUrl}{ApiConstants.News.NewsApiEndpoint}"
            };

            var response = await this.apiClientService.PostAsync<ApiRequest<NewsApiRequestModel>, NewsApiRequestModel, BaseResponseModel>(apiRequest);
            
            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }

            if (response.IsSuccessStatusCode && response.Data?.Id != null)
            {
                return response.Data.Id.Value;
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
                EndpointAddress = $"{this.settings.Value.NewsUrl}{ApiConstants.News.NewsApiEndpoint}/{id}"
            };

            var response = await this.apiClientService.DeleteAsync<ApiRequest<RequestModelBase>, RequestModelBase, BaseResponseModel>(apiRequest);
            
            if (response.IsSuccessStatusCode is false)
            {
                throw new CustomException(response.Message, (int)response.StatusCode);
            }
        }
    }
}
