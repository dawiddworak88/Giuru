using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using News.Api.Infrastructure;
using News.Api.Infrastructure.Entities.News;
using News.Api.ServicesModels.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace News.Api.Services.News
{
    public class NewsService : INewsService
    {
        private readonly NewsContext newsContext;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;

        public NewsService(
            NewsContext newsContext,
            IStringLocalizer<NewsResources> newsLocalizer)
        {
            this.newsContext = newsContext;
            this.newsLocalizer = newsLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateNewsItemServiceModel model)
        {
            var category = this.newsContext.Categories.FirstOrDefault(x => x.Id == model.CategoryId && x.IsActive);

            if (category is null)
            {
                throw new CustomException(this.newsLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            var newsItem = new NewsItem
            {
                ThumbnailImageId = model.ThumbnailImageId,
                PreviewImageId = model.PreviewImageId,
                OrganisationId = model.OrganisationId.Value,
                CategoryId = model.CategoryId.Value,
                IsPublished = model.IsPublished
            };

            await this.newsContext.NewsItems.AddAsync(newsItem.FillCommonProperties());

            var newsItemTranslation = new NewsItemTranslation
            {
                Title = model.Title,
                Description = model.Description,
                Content = model.Content,
                Language = model.Language,
                NewsItemId = newsItem.Id
            };

            await this.newsContext.NewsItemTranslations.AddAsync(newsItemTranslation.FillCommonProperties());

            foreach (var fileId in model.Files.OrEmptyIfNull())
            {
                var file = new NewsItemFile
                {
                    NewsItemId = newsItem.Id,
                    MediaId = fileId
                };

                await this.newsContext.NewsItemFiles.AddAsync(file.FillCommonProperties());
            }

            await this.newsContext.SaveChangesAsync();

            return newsItem.Id;
        }

        public async Task DeleteAsync(DeleteNewsItemServiceModel model)
        {
            var newsItem = this.newsContext.NewsItems.FirstOrDefault(x => x.Id == model.Id && x.IsActive);
            if (newsItem is null)
            {
                throw new CustomException(this.newsLocalizer.GetString("NewsNotFound"), (int)HttpStatusCode.NoContent);
            }

            newsItem.IsActive = false;

            await this.newsContext.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<NewsItemServiceModel>>> GetAsync(GetNewsItemsServiceModel model)
        {
            var news = this.newsContext.NewsItems.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                news = news.Where(x => x.Translations.Any(x => x.Title.StartsWith(model.SearchTerm)));
            }

            news = news.ApplySort(model.OrderBy);

            var pagedResults = news.PagedIndex(new Pagination(news.Count(), model.ItemsPerPage), model.PageIndex);

            var pagedNewsServiceModel = new PagedResults<IEnumerable<NewsItemServiceModel>>(pagedResults.Total, pagedResults.PageSize);

            var newsItems = new List<NewsItemServiceModel>();

            foreach (var newsItem in pagedResults.Data.OrEmptyIfNull().ToList())
            {
                var item = new NewsItemServiceModel
                {
                    Id = newsItem.Id,
                    ThumbnailImageId = newsItem.ThumbnailImageId,
                    PreviewImageId = newsItem.PreviewImageId,
                    CategoryId = newsItem.CategoryId,
                    IsPublished = newsItem.IsPublished,
                    LastModifiedDate = newsItem.LastModifiedDate,
                    CreatedDate = newsItem.CreatedDate
                };

                var files = this.newsContext.NewsItemFiles.Where(x => x.NewsItemId == newsItem.Id && x.IsActive);
                if (files.Any())
                {
                    item.Files = files.Select(x => x.MediaId);
                }

                var newsItemTranslations = this.newsContext.NewsItemTranslations.FirstOrDefault(x => x.Language == model.Language && x.NewsItemId == newsItem.Id && x.IsActive);
                if (newsItemTranslations is null)
                {
                    newsItemTranslations = this.newsContext.NewsItemTranslations.FirstOrDefault(x => x.NewsItemId == newsItem.Id && x.IsActive);
                }

                item.Title = newsItemTranslations?.Title;
                item.Description = newsItemTranslations?.Description;
                item.Content = newsItemTranslations?.Content;

                var newsCategoryTranslation = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == newsItem.CategoryId && x.IsActive);
                if (newsCategoryTranslation is null)
                {
                    newsCategoryTranslation = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.IsActive);
                }
                
                item.CategoryName = newsCategoryTranslation?.Name;

                newsItems.Add(item);
            }

            pagedNewsServiceModel.Data = newsItems;

            return pagedNewsServiceModel;
        }

        public async Task<NewsItemServiceModel> GetAsync(GetNewsItemServiceModel model)
        {
            var newsItem = this.newsContext.NewsItems.FirstOrDefault(x => x.Id == model.Id && x.IsActive);
            if (newsItem is not null)
            {
                var item = new NewsItemServiceModel
                {
                    Id = newsItem.Id,
                    ThumbnailImageId = newsItem.ThumbnailImageId,
                    PreviewImageId = newsItem.PreviewImageId,
                    CategoryId = newsItem.CategoryId,
                    IsPublished = newsItem.IsPublished,
                    LastModifiedDate = newsItem.LastModifiedDate,
                    CreatedDate = newsItem.CreatedDate
                };

                var newsItemTranslations = this.newsContext.NewsItemTranslations.FirstOrDefault(x => x.Language == model.Language && x.NewsItemId == newsItem.Id && x.IsActive);
                if (newsItemTranslations is null)
                {
                    newsItemTranslations = this.newsContext.NewsItemTranslations.FirstOrDefault(x => x.NewsItemId == newsItem.Id && x.IsActive);
                }

                item.Title = newsItemTranslations?.Title;
                item.Description = newsItemTranslations?.Description;
                item.Content = newsItemTranslations?.Content;

                var newsCategoryTranslation = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == newsItem.CategoryId && x.IsActive);
                if (newsCategoryTranslation is null)
                {
                    newsCategoryTranslation = this.newsContext.CategoryTranslations.FirstOrDefault(x => x.CategoryId == newsItem.CategoryId && x.IsActive);
                }
                
                item.CategoryName = newsCategoryTranslation?.Name;

                var files = this.newsContext.NewsItemFiles.Where(x => x.NewsItemId == newsItem.Id && x.IsActive);
                if (files.Any())
                {
                    item.Files = files.Select(x => x.MediaId);
                }

                return item;
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<NewsItemFileServiceModel>>> GetFilesAsync(GetNewsItemFilesServiceModel model)
        {
            var files = from f in this.newsContext.NewsItemFiles
                               where f.NewsItemId == model.Id && f.IsActive
                               select new NewsItemFileServiceModel
                               {
                                   Id = f.MediaId,
                                   LastModifiedDate = f.LastModifiedDate,
                                   CreatedDate = f.CreatedDate
                               };

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                files = files.Where(x => x.Id.ToString() == model.SearchTerm);
            }

            files = files.ApplySort(model.OrderBy);

            return files.PagedIndex(new Pagination(files.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<Guid> UpdateAsync(UpdateNewsItemServiceModel model)
        {
            var category = this.newsContext.Categories.FirstOrDefault(x => x.Id == model.CategoryId && x.IsActive);

            if (category is null)
            {
                throw new CustomException(this.newsLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            var news = this.newsContext.NewsItems.FirstOrDefault(x => x.Id == model.Id && x.IsActive);
            if (news is null)
            {
                throw new CustomException(this.newsLocalizer.GetString("NewsNotFound"), (int)HttpStatusCode.NoContent);
            }

            news.ThumbnailImageId = model.ThumbnailImageId;
            news.PreviewImageId = model.PreviewImageId;
            news.CategoryId = category.Id;
            news.IsPublished = model.IsPublished;
            news.LastModifiedDate = DateTime.UtcNow;

            var newsTranslation = this.newsContext.NewsItemTranslations.FirstOrDefault(x => x.NewsItemId == model.Id && x.Language == model.Language && x.IsActive);
            if (newsTranslation is not null)
            {
                newsTranslation.Title = model.Title;
                newsTranslation.Description = model.Description;
                newsTranslation.Content = model.Content;
                newsTranslation.LastModifiedDate = DateTime.UtcNow;
            } 
            else
            {
                var newNewsTranslation = new NewsItemTranslation
                {
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Language = model.Language,
                    NewsItemId = news.Id
                };

                await this.newsContext.NewsItemTranslations.AddAsync(newNewsTranslation.FillCommonProperties());
            }

            var newsFiles = this.newsContext.NewsItemFiles.Where(x => x.NewsItemId == news.Id);

            foreach(var newsFile in newsFiles.OrEmptyIfNull())
            {
                this.newsContext.NewsItemFiles.Remove(newsFile);
            }

            foreach(var fileId in model.Files.OrEmptyIfNull())
            {
                var file = new NewsItemFile
                {
                    MediaId = fileId,
                    NewsItemId = news.Id
                };

                await this.newsContext.NewsItemFiles.AddAsync(file.FillCommonProperties());
            }

            await this.newsContext.SaveChangesAsync();

            return news.Id;
        }
    }
}
