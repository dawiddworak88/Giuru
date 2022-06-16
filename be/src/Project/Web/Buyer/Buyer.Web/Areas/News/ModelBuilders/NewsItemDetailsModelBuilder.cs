using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Areas.Products.Repositories.Files;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Repositories.News;
using Buyer.Web.Shared.ViewModels.Files;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ModelBuilders
{
    public class NewsItemDetailsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsItemDetailsViewModel>
    {
        private readonly INewsRepository newsRepository;
        private readonly IMediaService mediaService;
        private readonly IMediaItemsRepository mediaRepository;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;

        public NewsItemDetailsModelBuilder(
            INewsRepository newsRepository,
            IMediaService mediaService,
            IMediaItemsRepository mediaRepository,
            IStringLocalizer<NewsResources> newsLocalizer)
        {
            this.newsRepository = newsRepository;
            this.mediaService = mediaService;
            this.mediaRepository = mediaRepository;
            this.newsLocalizer = newsLocalizer;
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
                    var files = new List<FileViewModel>();

                    foreach(var newsFile in existingNews.Files)
                    {
                        var file = await this.mediaRepository.GetMediaItemAsync(componentModel.Token, componentModel.Language, newsFile);

                        if (file is not null)
                        {
                            files.Add(new FileViewModel
                            {
                                Id = file.Id,
                                Url = this.mediaService.GetMediaUrl(newsFile),
                                Name = file.Name,
                                MimeType = file.MimeType,
                                Filename = file.Filename
                            });
                        };
                    };

                    viewModel.Files = files;
                }
            }

            return viewModel;
        }
    }
}
