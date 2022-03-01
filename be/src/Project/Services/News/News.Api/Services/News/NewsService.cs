using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using News.Api.Infrastructure;
using News.Api.Infrastructure.Entities.News;
using News.Api.ServicesModels.News;
using Newtonsoft.Json;
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
            var category = this.newsContext.Categories.FirstOrDefault(x => x.Id == model.CategoryId);

            if (category is null)
            {
                throw new CustomException(this.newsLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            var newsItem = new NewsItem
            {
                HeroImageId = model.HeroImageId.Value,
                OrganisationId = model.OrganisationId.Value,
                CategoryId = model.CategoryId.Value,
                IsNew = model.IsNew,
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
                    MediaId = fileId,
                };

                await this.newsContext.NewsItemFIles.AddAsync(file.FillCommonProperties());
            }

            foreach (var imageId in model.Images.OrEmptyIfNull())
            {
                var image = new NewsItemImage
                {
                    NewsItemId = newsItem.Id,
                    MediaId = imageId,
                };

                await this.newsContext.NewsItemImages.AddAsync(image.FillCommonProperties());
            }

            await this.newsContext.SaveChangesAsync();

            return newsItem.Id;
        }

        public async Task DeleteAsync(DeleteNewsItemServiceModel model)
        {
            var newsItem = this.newsContext.NewsItems.FirstOrDefault(x => x.Id == model.Id);
            if (newsItem is null)
            {
                throw new CustomException(this.newsLocalizer.GetString("NewsNotFound"), (int)HttpStatusCode.NotFound);
            }

            newsItem.IsActive = false;

            await this.newsContext.SaveChangesAsync();
        }

        public async Task<PagedResults<IEnumerable<NewsItemServiceModel>>> GetAsync(GetNewsItemsServiceModel model)
        {
            var existingNews = from n in this.newsContext.NewsItems
                       join nt in this.newsContext.NewsItemTranslations on n.Id equals nt.NewsItemId
                       join c in this.newsContext.CategoryTranslations on n.CategoryId equals c.CategoryId
                       where nt.Language == model.Language && n.IsActive && c.Language == model.Language
                       select new NewsItemServiceModel
                       {
                           Id = n.Id,
                           CategoryId = n.CategoryId,
                           CategoryName = c.Name,
                           Title = nt.Title,
                           Description = nt.Description,
                           Content = nt.Content,
                           IsPublished = n.IsPublished,
                           IsNew = n.IsNew,
                           LastModifiedDate = n.LastModifiedDate,
                           CreatedDate = n.CreatedDate
                       };

            foreach(var newsItem in existingNews)
            {
                var files = this.newsContext.NewsItemFIles.Where(x => x.NewsItemId == newsItem.Id);
                if (files is not null)
                {
                    newsItem.Files = files.Select(x => x.MediaId);
                }

                var images = this.newsContext.NewsItemImages.Where(x => x.NewsItemId == newsItem.Id);
                if (images is not null)
                {
                    newsItem.Images = images.Select(x => x.MediaId);
                }
            }

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                existingNews.Where(x => x.Title.StartsWith(model.SearchTerm));
            }

            existingNews.ApplySort(model.OrderBy);

            return existingNews.PagedIndex(new Pagination(existingNews.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<NewsItemServiceModel> GetAsync(GetNewsItemServiceModel model)
        {
            var existingNews = from n in this.newsContext.NewsItems
                               join nt in this.newsContext.NewsItemTranslations on n.Id equals nt.NewsItemId
                               join c in this.newsContext.CategoryTranslations on n.CategoryId equals c.CategoryId
                               where n.Id == model.Id && n.IsActive && nt.Language == model.Language && c.Language == model.Language
                               select new NewsItemServiceModel
                               {
                                   Id = n.Id,
                                   CategoryId = n.CategoryId,
                                   CategoryName = c.Name,
                                   Title = nt.Title,
                                   Description = nt.Description,
                                   Content = nt.Content,
                                   IsNew = n.IsNew,
                                   IsPublished = n.IsPublished,
                                   LastModifiedDate = n.LastModifiedDate,
                                   CreatedDate = n.CreatedDate
                               };

            var news = existingNews.FirstOrDefault();
            if (news is not null)
            {
                var files = this.newsContext.NewsItemFIles.Where(x => x.NewsItemId == model.Id);
                if (files is not null)
                {
                    news.Files = files.Select(x => x.MediaId);
                }

                var images = this.newsContext.NewsItemImages.Where(x => x.NewsItemId == model.Id);
                if (images is not null)
                {
                    news.Images = images.Select(x => x.MediaId);
                }

                return news;
            }

            return default;
        }

        public async Task<Guid> UpdateAsync(UpdateNewsItemServiceModel model)
        {
            var category = this.newsContext.Categories.FirstOrDefault(x => x.Id == model.CategoryId && x.IsActive);

            if (category is null)
            {
                throw new CustomException(this.newsLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NotFound);
            }

            var news = this.newsContext.NewsItems.FirstOrDefault(x => x.Id == model.Id && x.IsActive);
            if (news is null)
            {
                throw new CustomException(this.newsLocalizer.GetString("NewsNotFound"), (int)HttpStatusCode.NotFound);
            }

            news.HeroImageId = model.HeroImageId.Value;
            news.CategoryId = category.Id;
            news.IsNew = model.IsNew;
            news.IsPublished = model.IsPublished;
            news.LastModifiedDate = DateTime.UtcNow;

            var newsTranslation = this.newsContext.NewsItemTranslations.FirstOrDefault(x => x.Id == model.Id && x.Language == model.Language && x.IsActive);
            if (newsTranslation is not null)
            {
                newsTranslation.Title = model.Title;
                newsTranslation.Description = model.Description;
                newsTranslation.Content = model.Content;
                newsTranslation.LastModifiedDate = DateTime.UtcNow;
            } else
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

            var newsFiles = this.newsContext.NewsItemFIles.Where(x => x.NewsItemId == news.Id);

            foreach(var newsFile in newsFiles.OrEmptyIfNull())
            {
                this.newsContext.NewsItemFIles.Remove(newsFile);
            }

            foreach(var fileId in model.Files.OrEmptyIfNull())
            {
                var file = new NewsItemFile
                {
                    MediaId = fileId,
                    NewsItemId = news.Id
                };

                await this.newsContext.NewsItemFIles.AddAsync(file.FillCommonProperties());
            }

            /*var newsTags = this.newsContext.NewsItemTags.Where(x => x.NewsItemId == news.Id);

            foreach(var newsTag in newsTags.OrEmptyIfNull())
            {
                var existingTagTranslation = this.newsContext.NewsItemTagTranslations.FirstOrDefault(x => x.NewsItemTagId == news.Id && x.Language == model.Language);
                if (existingTagTranslation is not null)
                {
                    this.newsContext.NewsItemTagTranslations.Remove(existingTagTranslation);
                }

                this.newsContext.NewsItemTags.Remove(newsTag);
            }*/
            
            //dodaj tagi

            //dodaj images

            await this.newsContext.SaveChangesAsync();

            return news.Id;
        }

    }
}
