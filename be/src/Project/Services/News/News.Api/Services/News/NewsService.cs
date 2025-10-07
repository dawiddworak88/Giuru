using Foundation.Extensions.Exceptions;
using Foundation.Extensions.ExtensionMethods;
using Foundation.GenericRepository.Definitions;
using Foundation.GenericRepository.Extensions;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Microsoft.EntityFrameworkCore;
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
                throw new NotFoundException(_newsLocalizer.GetString("CategoryNotFound"));
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

            await _context.SaveChangesAsync();

            return newsItem.Id;
        }

        public async Task DeleteAsync(DeleteNewsItemServiceModel model)
        {
            var newsItem = _context.NewsItems.FirstOrDefault(x => x.Id == model.Id && x.IsActive);

            if (newsItem is null)
            {
                throw new NotFoundException(_newsLocalizer.GetString("NewsNotFound"));
            }

            newsItem.IsActive = false;

            await _context.SaveChangesAsync();
        }

        public PagedResults<IEnumerable<NewsItemServiceModel>> Get(GetNewsItemsServiceModel model)
        {
            var news = _context.NewsItems.Where(x => x.IsActive)
                    .Include(x => x.Files)
                    .Include(x => x.Category)
                    .Include(x => x.Category.Translations)
                    .Include(x => x.Translations)
                    .AsSingleQuery();

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

            return new PagedResults<IEnumerable<NewsItemServiceModel>>(pagedResults.Total, pagedResults.PageSize)
            {
                Data = pagedResults.Data.OrEmptyIfNull().Select(x => new NewsItemServiceModel
                {
                    Id = x.Id,
                    CategoryId = x.CategoryId,
                    Title = x.Translations.FirstOrDefault(t => t.NewsItemId == x.Id && t.Language == model.Language)?.Title ?? x.Translations.FirstOrDefault(t => t.NewsItemId == x.Id)?.Title,
                    Content = x.Translations.FirstOrDefault(t => t.NewsItemId == x.Id && t.Language == model.Language)?.Content ?? x.Translations.FirstOrDefault(t => t.NewsItemId == x.Id)?.Content,
                    Description = x.Translations.FirstOrDefault(t => t.NewsItemId == x.Id && t.Language == model.Language)?.Description ?? x.Translations.FirstOrDefault(t => t.NewsItemId == x.Id)?.Description,
                    Language = model.Language,
                    OrganisationId = x.OrganisationId,
                    CategoryName = x.Category.Translations.FirstOrDefault(t => t.CategoryId == x.CategoryId && t.Language == model.Language)?.Name ?? x.Category.Translations.FirstOrDefault(t => t.CategoryId == x.CategoryId)?.Name,
                    PreviewImageId = x.PreviewImageId,
                    ThumbnailImageId = x.ThumbnailImageId,
                    IsPublished = x.IsPublished,
                    Files = x.Files.Select(x => x.Id),
                    LastModifiedDate = x.LastModifiedDate,
                    CreatedDate = x.CreatedDate
                })
            };
        }

        public async Task<NewsItemServiceModel> GetAsync(GetNewsItemServiceModel model)
        {
            var newsItem = await _context.NewsItems
                    .Include(x => x.Category)
                    .Include(x => x.Category.Translations)
                    .Include(x => x.Translations)
                    .AsSingleQuery()
                    .FirstOrDefaultAsync(x => x.Id == model.Id && x.IsActive);

            if (newsItem is null)
            {
                throw new NotFoundException(_newsLocalizer.GetString("NewsNotFound"));
            }

            return new NewsItemServiceModel
            {
                Id = newsItem.Id,
                CategoryId = newsItem.CategoryId,
                CategoryName = newsItem.Category?.Translations?.FirstOrDefault(t => t.CategoryId == newsItem.CategoryId && t.Language == model.Language && t.IsActive)?.Name ?? newsItem.Category?.Translations?.FirstOrDefault(t => t.CategoryId == newsItem.CategoryId && t.IsActive)?.Name,
                Title = newsItem.Translations.FirstOrDefault(t => t.NewsItemId == newsItem.Id && t.IsActive && t.Language == model.Language)?.Title ?? newsItem.Translations.FirstOrDefault(t => t.NewsItemId == newsItem.Id && t.IsActive)?.Title,
                Description = newsItem.Translations.FirstOrDefault(t => t.NewsItemId == newsItem.Id && t.IsActive && t.Language == model.Language)?.Description ?? newsItem.Translations.FirstOrDefault(t => t.NewsItemId == newsItem.Id && t.IsActive)?.Description,
                Content = newsItem.Translations.FirstOrDefault(t => t.NewsItemId == newsItem.Id && t.IsActive && t.Language == model.Language)?.Content ?? newsItem.Translations.FirstOrDefault(t => t.NewsItemId == newsItem.Id && t.IsActive)?.Content,
                PreviewImageId = newsItem.PreviewImageId,
                ThumbnailImageId = newsItem.ThumbnailImageId,
                IsPublished = newsItem.IsPublished,
                Files = newsItem.Files.Where(f => f.NewsItemId == newsItem.Id && f.IsActive).OrEmptyIfNull().Select(f => f.MediaId),
                LastModifiedDate = newsItem.LastModifiedDate,
                CreatedDate = newsItem.CreatedDate
            };
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
                throw new NotFoundException(_newsLocalizer.GetString("CategoryNotFound"));
            }

            var news = _context.NewsItems.FirstOrDefault(x => x.Id == model.Id && x.IsActive);
            if (news is null)
            {
                throw new NotFoundException(_newsLocalizer.GetString("NewsNotFound"));
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

                await _context.NewsItemFiles.AddAsync(file.FillCommonProperties());
            }

            await _context.SaveChangesAsync();

            return news.Id;
        }
    }
}
