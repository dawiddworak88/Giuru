using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Seller.Web.Areas.News.Repositories.Categories;
using Seller.Web.Areas.News.Repositories.News;
using Seller.Web.Areas.News.ViewModel;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Shared.Configurations;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Seller.Web.Areas.News.ModelBuilders
{
    public class NewsItemFormModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsItemFormViewModel>
    {
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;
        private readonly LinkGenerator linkGenerator;
        private readonly ICategoriesRepository categoriesRepository;
        private readonly IMediaItemsRepository mediaItemsRepository;
        private readonly IMediaHelperService mediaHelperService;
        private readonly INewsRepository newsRepository;
        private readonly IOptionsMonitor<AppSettings> settings;

        public NewsItemFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<NewsResources> newsLocalizer,
            ICategoriesRepository categoriesRepository,
            IMediaItemsRepository mediaItemsRepository,
            INewsRepository newsRepository,
            IMediaHelperService mediaHelperService,
            IOptionsMonitor<AppSettings> settings,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.newsLocalizer = newsLocalizer;
            this.categoriesRepository = categoriesRepository;
            this.newsRepository = newsRepository;
            this.mediaItemsRepository = mediaItemsRepository;
            this.settings = settings;
            this.mediaHelperService = mediaHelperService;
        }

        public async Task<NewsItemFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsItemFormViewModel
            {
                Title = this.newsLocalizer.GetString("NewsItem"),
                SaveText = this.globalLocalizer.GetString("SaveText"),
                SaveUrl = this.linkGenerator.GetPathByAction("Post", "NewsApi", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name }),
                SaveMediaUrl = this.linkGenerator.GetPathByAction("Post", "FilesApi", new { Area = "Media", culture = CultureInfo.CurrentUICulture.Name }),
                DeleteLabel = this.globalLocalizer.GetString("Delete"),
                GeneralErrorMessage = this.globalLocalizer.GetString("AnErrorOccurred"),
                DropOrSelectImagesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                DropFilesLabel = this.globalLocalizer.GetString("DropOrSelectFile"),
                NewsUrl = this.linkGenerator.GetPathByAction("Index", "News", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name }),
                NavigateToNewsLabel = this.newsLocalizer.GetString("NavigateToNewsLabel"),
                HeroImageLabel = this.newsLocalizer.GetString("HeroImageLabel"),
                TitleLabel = this.newsLocalizer.GetString("TitleLabel"),
                DescriptionLabel = this.newsLocalizer.GetString("DescriptionLabel"),
                IsNewLabel = this.newsLocalizer.GetString("IsNewLabel"),
                IsPublishedLabel = this.newsLocalizer.GetString("IsPublishedLabel"),
                ImagesLabel = this.newsLocalizer.GetString("ImagesLabel"),
                FilesLabel = this.newsLocalizer.GetString("FilesLabel"),
                CategoryLabel = this.newsLocalizer.GetString("Category"),
                SelectCategoryLabel = this.newsLocalizer.GetString("SelectCategoryLabel"),
                ThumbImageLabel = this.newsLocalizer.GetString("ThumbImageLabel"),
                TitleRequiredErrorMessage = this.newsLocalizer.GetString("TitleRequiredErrorMessage"),
                CategoryRequiredErrorMessage = this.newsLocalizer.GetString("CategoryRequiredErrorMessage"),
                DescriptionRequiredErrorMessage = this.newsLocalizer.GetString("DescriptionRequiredErrorMessage")
            };

            var categories = await this.categoriesRepository.GetAllCategoriesAsync(componentModel.Token, componentModel.Language);
            if (categories is not null)
            {
                viewModel.Categories = categories.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            if (componentModel.Id.HasValue)
            {
                var existingNews = await this.newsRepository.GetAsync(componentModel.Token, componentModel.Language, componentModel.Id);
                if (existingNews is not null)
                { 

                    if (existingNews.Files is not null && existingNews.Files.Any())
                    {
                        var fileMediaItems = await this.mediaItemsRepository.GetAllMediaItemsAsync(
                            componentModel.Token, componentModel.Language, existingNews.Files.Distinct().ToEndpointParameterString(), PaginationConstants.DefaultPageIndex, PaginationConstants.DefaultPageSize);

                        var files = new List<FileViewModel>();
                        foreach (var file in fileMediaItems)
                        {
                            files.Add(new FileViewModel
                            {
                                Id = file.Id,
                                Url = this.mediaHelperService.GetFileUrl(this.settings.CurrentValue.MediaUrl, file.Id, Constants.PreviewMaxWidth, Constants.PreviewMaxHeight, true),
                                Name = file.Name,
                                MimeType = file.MimeType,
                                Filename = file.Filename,
                                Extension = file.Extension
                            });
                        }

                        viewModel.Files = files;
                    }

                    var thumbImage = await this.mediaItemsRepository.GetMediaItemAsync(componentModel.Token, componentModel.Language, existingNews.ThumbImageId);
                    if (thumbImage is not null)
                    {

                        viewModel.ThumbImages = new List<FileViewModel>
                        {
                            new FileViewModel
                            {
                                Id = thumbImage.Id,
                                Url = this.mediaHelperService.GetFileUrl(this.settings.CurrentValue.MediaUrl, thumbImage.Id, Constants.PreviewMaxWidth, Constants.PreviewMaxHeight, true),
                                Name = thumbImage.Name,
                                MimeType = thumbImage.MimeType,
                                Filename = thumbImage.Filename,
                                Extension = thumbImage.Extension
                            }
                        };

                    }

                    var heroImage = await this.mediaItemsRepository.GetMediaItemAsync(componentModel.Token, componentModel.Language, existingNews.HeroImageId);
                    if (heroImage is not null)
                    {
                        viewModel.HeroImages = new List<FileViewModel>
                        {
                            new FileViewModel
                            {
                                Id = heroImage.Id,
                                Url = this.mediaHelperService.GetFileUrl(this.settings.CurrentValue.MediaUrl, heroImage.Id, Constants.PreviewMaxWidth, Constants.PreviewMaxHeight, true),
                                Name = heroImage.Name,
                                MimeType = heroImage.MimeType,
                                Filename = heroImage.Filename,
                                Extension = heroImage.Extension
                            }
                        };
                    }

                    viewModel.Id = componentModel.Id;
                    viewModel.CategoryId = existingNews.CategoryId;
                    viewModel.NewsTitle = existingNews.Title;
                    viewModel.Content = existingNews.Content;
                    viewModel.Description = existingNews.Description;
                    viewModel.IsPublished = existingNews.IsPublished;
                    
                }
            }

            return viewModel;
        }
    }
}
