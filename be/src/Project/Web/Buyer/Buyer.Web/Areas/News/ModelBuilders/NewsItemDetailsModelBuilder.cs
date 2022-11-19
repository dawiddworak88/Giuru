using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Areas.Products.DomainModels;
using Buyer.Web.Areas.Products.Repositories.Files;
using Buyer.Web.Shared.ComponentModels.Files;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Definitions.Files;
using Buyer.Web.Shared.Repositories.News;
using Buyer.Web.Shared.ViewModels.Files;
using Foundation.Extensions.ExtensionMethods;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ModelBuilders
{
    public class NewsItemDetailsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsItemDetailsViewModel>
    {
        private readonly IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder;
        private readonly INewsRepository newsRepository;
        private readonly IMediaService mediaService;
        private readonly IMediaItemsRepository mediaRepository;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;
        private readonly LinkGenerator linkGenerator;

        public NewsItemDetailsModelBuilder(
            IAsyncComponentModelBuilder<FilesComponentModel, FilesViewModel> filesModelBuilder,
            INewsRepository newsRepository,
            IMediaService mediaService,
            IMediaItemsRepository mediaRepository,
            IStringLocalizer<NewsResources> newsLocalizer,
            LinkGenerator linkGenerator)
        {
            this.filesModelBuilder = filesModelBuilder;
            this.newsRepository = newsRepository;
            this.mediaService = mediaService;
            this.mediaRepository = mediaRepository;
            this.newsLocalizer = newsLocalizer;
            this.linkGenerator = linkGenerator;
        }

        public async Task<NewsItemDetailsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsItemDetailsViewModel
            {
                FilesLabel = this.newsLocalizer.GetString("FilesLabel")
            };

            var existingNews = await this.newsRepository.GetNewsItemAsync(componentModel.Token, componentModel.Language, componentModel.Id);
            if (existingNews is not null)
            {
                viewModel.Id = existingNews.Id;
                viewModel.Title = existingNews.Title;
                viewModel.Content = existingNews.Content;
                viewModel.Description = existingNews.Description;
                viewModel.CreatedDate = existingNews.CreatedDate;
                viewModel.CategoryName = existingNews.CategoryName;
                
                if (existingNews.PreviewImageId.HasValue)
                {
                    viewModel.PreviewImageUrl = this.mediaService.GetMediaUrl(existingNews.PreviewImageId.Value, 1024);
                    viewModel.PreviewImages = new List<SourceViewModel>
                    {
                        new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(existingNews.PreviewImageId.Value, 1024) },
                        new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(existingNews.PreviewImageId.Value, 352) },
                        new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(existingNews.PreviewImageId.Value, 608) },
                        new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(existingNews.PreviewImageId.Value, 768) }
                    };
                }

                if (existingNews.Files is not null)
                {
                    var files = await this.newsRepository.GetNewsItemFilesAsync(componentModel.Token, componentModel.Language, componentModel.Id, FilesConstants.DefaultPageIndex, FilesConstants.DefaultPageSize, null, $"{nameof(ProductFile.CreatedDate)} desc");

                    if (files is not null)
                    {
                        var fileComponentModel = new FilesComponentModel
                        {
                            Id = componentModel.Id,
                            IsAuthenticated = componentModel.IsAuthenticated,
                            Language = componentModel.Language,
                            Token = componentModel.Token,
                            SearchApiUrl = this.linkGenerator.GetPathByAction("GetFiles", "NewsApi", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name }),
                            Files = files.Data.OrEmptyIfNull().Select(x => x.Id)
                        };

                        viewModel.Files = await this.filesModelBuilder.BuildModelAsync(fileComponentModel);
                    }
                }
            }

            return viewModel;
        }
    }
}
