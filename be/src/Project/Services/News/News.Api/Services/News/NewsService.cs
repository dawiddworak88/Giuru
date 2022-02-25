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

            foreach (var tagString in model.Tags.OrEmptyIfNull())
            {
                var tag = new NewsItemTag
                {
                    NewsItemId = newsItem.Id
                };

                await this.newsContext.NewsItemTags.AddAsync(tag.FillCommonProperties());

                var tagTranslation = new NewsItemTagTranslation
                {
                    Name = tagString,
                    Language = model.Language,
                    NewsItemTagId = tag.Id
                };

                await this.newsContext.NewsItemTagTranslations.AddAsync(tagTranslation.FillCommonProperties());
            }

            foreach (var fileId in model.Images.OrEmptyIfNull())
            {
                var image = new NewsItemFile
                {
                    NewsItemId = newsItem.Id
                };

                await this.newsContext.NewsItemFIles.AddAsync(image.FillCommonProperties());
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
            var news = from n in this.newsContext.NewsItems
                       join nt in this.newsContext.NewsItemTranslations on n.Id equals nt.NewsItemId
                       join t in this.newsContext.NewsItemTags on n.Id equals t.NewsItemId
                       where nt.Language == model.Language && n.IsActive
                       select new NewsItemServiceModel
                       {
                           Id = n.Id,
                           CategoryId = n.CategoryId,
                           Title = nt.Title,
                           Description = nt.Description,
                           Content = nt.Content,
                           IsPublished = n.IsPublished,
                           IsNew = n.IsNew,
                           LastModifiedDate = DateTime.UtcNow,
                           CreatedDate = DateTime.UtcNow,
                       };

            foreach(var newsItem in news)
            {

            }

            if (!string.IsNullOrWhiteSpace(model.SearchTerm))
            {
                news = news.Where(x => x.Title.StartsWith(model.SearchTerm));
            }

            news = news.ApplySort(model.OrderBy);

            return news.PagedIndex(new Pagination(news.Count(), model.ItemsPerPage), model.PageIndex);
        }

        public async Task<NewsItemServiceModel> GetAsync(GetNewsItemServiceModel model)
        {
            var news = from n in this.newsContext.NewsItems
                       join nt in this.newsContext.NewsItemTranslations on n.Id equals nt.NewsItemId
                       join t in this.newsContext.NewsItemTags on n.Id equals t.NewsItemId
                       join tt in this.newsContext.NewsItemTagTranslations on t.Id equals tt.NewsItemTagId
                       join f in this.newsContext.NewsItemFIles on n.Id equals f.NewsItemId
                       where n.Id == model.Id
                       select new
                       {
                           Id = n.Id,
                           CategoryId = n.CategoryId,
                           Title = nt.Title,
                           Description = nt.Description,
                           Content = nt.Content,
                           IsNew = n.IsNew,
                           Tags = tt.Name,
                           Files = f.MediaId,
                           IsPublished = n.IsPublished,
                           LastModifiedDate = n.LastModifiedDate,
                           CreatedDate = n.CreatedDate
                       };

            if (news.OrEmptyIfNull().Any())
            {
                var newsItem = new NewsItemServiceModel
                {
                    Id = model.Id,
                    CategoryId = news.FirstOrDefault().CategoryId,
                    Title = news.FirstOrDefault().Title,
                    Description = news.FirstOrDefault().Description,
                    Content = news.FirstOrDefault().Content,
                    Tags = news.Select(tag => tag.Tags.ToString()),
                    Files = news.Select(x => x.Files),
                    IsNew = news.FirstOrDefault().IsNew,
                    IsPublished = news.FirstOrDefault().IsPublished,
                    LastModifiedDate = news.FirstOrDefault().LastModifiedDate,
                    CreatedDate = news.FirstOrDefault().CreatedDate
                };

                return newsItem;
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

            var newsTags = this.newsContext.NewsItemTags.Where(x => x.NewsItemId == news.Id);

            foreach(var newsTag in newsTags.OrEmptyIfNull())
            {
                var existingTagTranslation = this.newsContext.NewsItemTagTranslations.FirstOrDefault(x => x.NewsItemTagId == news.Id && x.Language == model.Language);
                if (existingTagTranslation is not null)
                {
                    this.newsContext.NewsItemTagTranslations.Remove(existingTagTranslation);
                }

                this.newsContext.NewsItemTags.Remove(newsTag);
            }
            
            //dodaj tagi

            //dodaj images

            await this.newsContext.SaveChangesAsync();

            return news.Id;
        }

    }
}
