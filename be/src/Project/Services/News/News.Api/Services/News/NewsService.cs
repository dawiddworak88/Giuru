using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
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
        private readonly NewsContext _context;
        private readonly IStringLocalizer<NewsResources> _newsLocalizer;

        public NewsService(
            NewsContext context,
            IStringLocalizer<NewsResources> newsLocalizer)
        {
            _context = context;
            _newsLocalizer = newsLocalizer;
        }

        public async Task<Guid> CreateAsync(CreateNewsItemServiceModel model)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == model.CategoryId && x.IsActive);

            if (category is null)
            {
                throw new CustomException(_newsLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            var newsItem = new NewsItem
            {
                ThumbnailImageId = model.ThumbnailImageId,
                PreviewImageId = model.PreviewImageId,
                OrganisationId = model.OrganisationId.Value,
                CategoryId = model.CategoryId.Value,
                IsPublished = model.IsPublished
            };

            await _context.NewsItems.AddAsync(newsItem.FillCommonProperties());

            var newsItemTranslation = new NewsItemTranslation
            {
                Title = model.Title,
                Description = model.Description,
                Content = model.Content,
                Language = model.Language,
                NewsItemId = newsItem.Id
            };

            await _context.NewsItemTranslations.AddAsync(newsItemTranslation.FillCommonProperties());

            foreach (var fileId in model.Files.OrEmptyIfNull())
            {
                var file = new NewsItemFile
                {
                    NewsItemId = newsItem.Id,
                    MediaId = fileId
                };

                await _context.NewsItemFiles.AddAsync(file.FillCommonProperties());
            }

            foreach (var clientGroupId in model.ClientGroupIds.OrEmptyIfNull())
            {
                var group = new NewsItemsGroup
                {
                    NewsItemId = newsItem.Id,
                    GroupId = clientGroupId
                };

                await this.newsContext.NewsItemsGroups.AddAsync(group.FillCommonProperties());
            }

            await _context.SaveChangesAsync();

            return newsItem.Id;
        }

        public async Task DeleteAsync(DeleteNewsItemServiceModel model)
        {
            var newsItem = _context.NewsItems.FirstOrDefault(x => x.Id == model.Id && x.IsActive);
            
            if (newsItem is null)
            {
                throw new CustomException(_newsLocalizer.GetString("NewsNotFound"), (int)HttpStatusCode.NoContent);
            }

            newsItem.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<NewsItemServiceModel>> Get(GetNewsItemsServiceModel model)
        {
            var news = _context.NewsItems.Where(x => x.IsActive);

            if (string.IsNullOrWhiteSpace(model.SearchTerm) is false)
            {
                news = news.Where(x => x.Translations.Any(x => x.Language == model.Language && x.Title.StartsWith(model.SearchTerm) && x.IsActive));
            }

            news = news.ApplySort(model.OrderBy);

            PagedResults<IEnumerable<NewsItem>> pagedResults;

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                news = news.Take(Constants.MaxItemsPerPageLimit);

                pagedResults = news.PagedIndex(new Pagination(news.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }
            else
            {
                pagedResults = news.PagedIndex(new Pagination(news.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
            }

            var translations = _context.NewsItemTranslations.Where(x => pagedResults.Data.Select(y => y.Id).Contains(x.NewsItemId) && x.IsActive && x.IsActive).ToList();

            var categoryTranslations = _context.CategoryTranslations.Where(x => pagedResults.Data.Select(y => y.CategoryId).Contains(x.CategoryId)).ToList();

            var files = _context.NewsItemFiles.Where(x => pagedResults.Data.Select(y => y.Id).Contains(x.NewsItemId) && x.IsActive).ToList();

            var clientGroups = _context.NewsItemsGroups.Where(x => x.pagedResults.Data.Select(y => y.Id).Contains(x.NewsItemId) && x.IsActive).ToList();

            return new PagedResults<IEnumerable<NewsItemServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new NewsItemServiceModel
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    Title = translations.FirstOrDefault(t => t.NewsItemId == x.Id && t.Language == model.Language)?.Title ?? translations.FirstOrDefault(t => t.NewsItemId == x.Id)?.Title,
                    Content = translations.FirstOrDefault(t => t.NewsItemId == x.Id && t.Language == model.Language)?.Content ?? translations.FirstOrDefault(t => t.NewsItemId == x.Id)?.Content,
                    Description = translations.FirstOrDefault(t => t.NewsItemId == x.Id && t.Language == model.Language)?.Description ?? translations.FirstOrDefault(t => t.NewsItemId == x.Id)?.Description,
                    Language = model.Language,
                    OrganisationId = x.OrganisationId,
                    CategoryName = categoryTranslations.FirstOrDefault(t => t.CategoryId == x.CategoryId && t.Language == model.Language)?.Name ?? categoryTranslations.FirstOrDefault(t => t.CategoryId == x.CategoryId)?.Name,
                    PreviewImageId = x.PreviewImageId,
                    ThumbnailImageId = x.ThumbnailImageId,
                    IsPublished = x.IsPublished,
                    Files = files.Select(x => x.Id),
                    ClientGroupIds = clientGroups.Select(x => x.Id);
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task<NewsItemServiceModel> GetAsync(GetNewsItemServiceModel model)
        {
            var newsItem = _context.NewsItems.FirstOrDefault(x => x.Id == model.Id && x.IsActive);
            
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

                var newsItemTranslations = _context.NewsItemTranslations.FirstOrDefault(x => x.Language == model.Language && x.NewsItemId == newsItem.Id && x.IsActive);
                
                if (newsItemTranslations is null)
                {
                    newsItemTranslations = _context.NewsItemTranslations.FirstOrDefault(x => x.NewsItemId == newsItem.Id && x.IsActive);
                }

                item.Title = newsItemTranslations?.Title;
                item.Description = newsItemTranslations?.Description;
                item.Content = newsItemTranslations?.Content;

                var newsCategoryTranslation = _context.CategoryTranslations.FirstOrDefault(x => x.Language == model.Language && x.CategoryId == newsItem.CategoryId && x.IsActive);
               
                if (newsCategoryTranslation is null)
                {
                    newsCategoryTranslation = _context.CategoryTranslations.FirstOrDefault(x => x.CategoryId == newsItem.CategoryId && x.IsActive);
                }
                
                item.CategoryName = newsCategoryTranslation?.Name;

                var files = _context.NewsItemFiles.Where(x => x.NewsItemId == newsItem.Id && x.IsActive);
               
               if (files.Any())
                {
                    item.Files = files;
                }

                var clientGroups = _context.NewsItemsGroups.Where(x => x.NewsItemId == newsItem.Id && x.IsActive).Select(x => x.GroupId);

                if (clientGroups is not null)
                {
                    item.ClientGroupIds = clientGroups;
                }

                return item;
            }

            return default;
        }

        public async Task<PagedResults<IEnumerable<NewsItemFileServiceModel>>> GetFilesAsync(GetNewsItemFilesServiceModel model)
        {
            var files = from f in _context.NewsItemFiles
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

            if (model.PageIndex.HasValue is false || model.ItemsPerPage.HasValue is false)
            {
                files = files.Take(Constants.MaxItemsPerPageLimit);

                return files.PagedIndex(new Pagination(files.Count(), Constants.MaxItemsPerPageLimit), Constants.DefaultPageIndex);
            }

            return files.PagedIndex(new Pagination(files.Count(), model.ItemsPerPage.Value), model.PageIndex.Value);
        }

        public async Task<Guid> UpdateAsync(UpdateNewsItemServiceModel model)
        {
            var category = _context.Categories.FirstOrDefault(x => x.Id == model.CategoryId && x.IsActive);

            if (category is null)
            {
                throw new CustomException(_newsLocalizer.GetString("CategoryNotFound"), (int)HttpStatusCode.NoContent);
            }

            var news = _context.NewsItems.FirstOrDefault(x => x.Id == model.Id && x.IsActive);
            
            if (news is null)
            {
                throw new CustomException(_newsLocalizer.GetString("NewsNotFound"), (int)HttpStatusCode.NoContent);
            }

            news.ThumbnailImageId = model.ThumbnailImageId;
            news.PreviewImageId = model.PreviewImageId;
            news.CategoryId = category.Id;
            news.IsPublished = model.IsPublished;
            news.LastModifiedDate = DateTime.UtcNow;

            var newsTranslation = _context.NewsItemTranslations.FirstOrDefault(x => x.NewsItemId == model.Id && x.Language == model.Language && x.IsActive);
            
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

                await _context.NewsItemTranslations.AddAsync(newNewsTranslation.FillCommonProperties());
            }

            var newsFiles = _context.NewsItemFiles.Where(x => x.NewsItemId == news.Id);

            foreach(var newsFile in newsFiles.OrEmptyIfNull())
            {
                _context.NewsItemFiles.Remove(newsFile);
            }

            foreach(var fileId in model.Files.OrEmptyIfNull())
            {
                var file = new NewsItemFile
                {
                    MediaId = fileId,
                    NewsItemId = news.Id
                };

                _context.NewsItemFiles.AddAsync(file.FillCommonProperties());
            }

            var clientGroups = _context.NewsItemsGroups.Where(x => x.NewsItemId == news.Id);

            foreach (var clientGroup in clientGroups.OrEmptyIfNull())
            {
                _context.NewsItemsGroups.Remove(clientGroup);
            }

            foreach (var clientGroupId in model.ClientGroupIds.OrEmptyIfNull())
            {
                var group = new NewsItemsGroup
                {
                    NewsItemId = news.Id,
                    GroupId = clientGroupId
                };

                _context.NewsItemsGroups.AddAsync(group.FillCommonProperties());
            }

            await _context.SaveChangesAsync();

            return news.Id;
        }
    }
}
