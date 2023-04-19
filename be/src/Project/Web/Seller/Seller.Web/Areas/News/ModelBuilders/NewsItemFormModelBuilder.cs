using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.GenericRepository.Paginations;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Seller.Web.Areas.News.Repositories.Categories;
using Seller.Web.Areas.News.Repositories.News;
using Seller.Web.Areas.News.ViewModel;
using Seller.Web.Areas.Shared.Repositories.Media;
using Seller.Web.Shared.Definitions;
using Seller.Web.Shared.Repositories.Clients;
using Seller.Web.Shared.ViewModels;
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
        private readonly IMediaService mediaService;
        private readonly INewsRepository newsRepository;
        private readonly IClientGroupsRepository clientGroupsRepository;

        public NewsItemFormModelBuilder(
            IStringLocalizer<GlobalResources> globalLocalizer,
            IStringLocalizer<NewsResources> newsLocalizer,
            ICategoriesRepository categoriesRepository,
            IMediaItemsRepository mediaItemsRepository,
            INewsRepository newsRepository,
            IMediaService mediaService,
            IClientGroupsRepository clientGroupsRepository,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.globalLocalizer = globalLocalizer;
            this.newsLocalizer = newsLocalizer;
            this.categoriesRepository = categoriesRepository;
            this.newsRepository = newsRepository;
            this.mediaItemsRepository = mediaItemsRepository;
            this.mediaService = mediaService;
            this.clientGroupsRepository = clientGroupsRepository;
        }

        public async Task<NewsItemFormViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsItemFormViewModel
            {
                IdLabel = this.globalLocalizer.GetString("Id"),
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
                PreviewImageLabel = this.newsLocalizer.GetString("PreviewImageLabel"),
                TitleLabel = this.newsLocalizer.GetString("TitleLabel"),
                DescriptionLabel = this.newsLocalizer.GetString("DescriptionLabel"),
                IsPublishedLabel = this.newsLocalizer.GetString("IsPublishedLabel"),
                ImagesLabel = this.newsLocalizer.GetString("ImagesLabel"),
                FilesLabel = this.newsLocalizer.GetString("FilesLabel"),
                CategoryLabel = this.newsLocalizer.GetString("Category"),
                SelectCategoryLabel = this.newsLocalizer.GetString("SelectCategoryLabel"),
                ThumbImageLabel = this.newsLocalizer.GetString("ThumbImageLabel"),
                TitleRequiredErrorMessage = this.newsLocalizer.GetString("TitleRequiredErrorMessage"),
                CategoryRequiredErrorMessage = this.newsLocalizer.GetString("CategoryRequiredErrorMessage"),
                DescriptionRequiredErrorMessage = this.newsLocalizer.GetString("DescriptionRequiredErrorMessage"),
                NoGroupsText = this.globalLocalizer.GetString("NoGroupsText"),
                GroupsLabel = this.globalLocalizer.GetString("Groups")
            };

            var categories = await this.categoriesRepository.GetAllCategoriesAsync(componentModel.Token, componentModel.Language);

            if (categories is not null)
            {
                viewModel.Categories = categories.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
            }

            var groups = await this.clientGroupsRepository.GetAsync(componentModel.Token, componentModel.Language);

            if (groups is not null)
            {
                viewModel.Groups = groups.Select(x => new ListItemViewModel { Id = x.Id, Name = x.Name });
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
                                Url = this.mediaService.GetMediaUrl(file.Id, Constants.PreviewMaxWidth),
                                Name = file.Name,
                                MimeType = file.MimeType,
                                Filename = file.Filename,
                                Extension = file.Extension
                            });
                        }

                        viewModel.Files = files;
                    }

                    if (existingNews.ThumbnailImageId.HasValue)
                    {
                        var thumbnailImage = await this.mediaItemsRepository.GetMediaItemAsync(componentModel.Token, componentModel.Language, existingNews.ThumbnailImageId.Value);

                        if (thumbnailImage is not null)
                        {

                            viewModel.ThumbnailImages = new List<FileViewModel>
                            {
                                new FileViewModel
                                {
                                    Id = thumbnailImage.Id,
                                    Url = this.mediaService.GetMediaUrl(thumbnailImage.Id, Constants.PreviewMaxWidth),
                                    Name = thumbnailImage.Name,
                                    MimeType = thumbnailImage.MimeType,
                                    Filename = thumbnailImage.Filename,
                                    Extension = thumbnailImage.Extension
                                }
                            };
                        }
                    }

                    if (existingNews.PreviewImageId.HasValue)
                    {
                        var previewImage = await this.mediaItemsRepository.GetMediaItemAsync(componentModel.Token, componentModel.Language, existingNews.PreviewImageId.Value);

                        if (previewImage is not null)
                        {
                            viewModel.PreviewImages = new List<FileViewModel>
                            {
                                new FileViewModel
                                {
                                    Id = previewImage.Id,
                                    Url = this.mediaService.GetMediaUrl(previewImage.Id, Constants.PreviewMaxWidth),
                                    Name = previewImage.Name,
                                    MimeType = previewImage.MimeType,
                                    Filename = previewImage.Filename,
                                    Extension = previewImage.Extension
                                }
                            };
                        }
                    }

                    viewModel.Id = componentModel.Id;
                    viewModel.CategoryId = existingNews.CategoryId;
                    viewModel.NewsTitle = existingNews.Name;
                    viewModel.Content = existingNews.Content;
                    viewModel.Description = existingNews.Description;
                    viewModel.IsPublished = existingNews.IsPublished;
                    viewModel.ClientGroupIds = existingNews.ClientGroupIds;
                }
            }

            return viewModel;
        }
    }
}
