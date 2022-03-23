using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Areas.Products.Repositories.Files;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Repositories.News;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;
using Buyer.Web.Shared.ViewModels.Files;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ModelBuilders
{
    public class NewsItemDetailsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsItemDetailsViewModel>
    {
        private readonly IOptions<AppSettings> options;
        private readonly LinkGenerator linkGenerator;
        private readonly INewsRepository newsRepository;
        private readonly ICdnService cdnService;
        private readonly IMediaHelperService mediaService;
        private readonly IMediaItemsRepository mediaRepository;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;

        public NewsItemDetailsModelBuilder(
            IOptions<AppSettings> options,
            INewsRepository newsRepository,
            ICdnService cdnService,
            IMediaHelperService mediaService,
            IMediaItemsRepository mediaRepository,
            IStringLocalizer<NewsResources> newsLocalizer,
            LinkGenerator linkGenerator)
        {
            this.options = options;
            this.linkGenerator = linkGenerator;
            this.newsRepository = newsRepository;
            this.cdnService = cdnService;
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
                    viewModel.PreviewImageUrl = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, existingNews.PreviewImageId.Value, 1024, 1024, true, MediaConstants.WebpExtension));
                    viewModel.PreviewImages = new List<SourceViewModel>
                    {
                        new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, existingNews.PreviewImageId.Value, 1024, 1024, true, MediaConstants.WebpExtension)) },
                        new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, existingNews.PreviewImageId.Value, 352, 352, true,MediaConstants.WebpExtension)) },
                        new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, existingNews.PreviewImageId.Value, 608, 608, true, MediaConstants.WebpExtension)) },
                        new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, existingNews.PreviewImageId.Value, 768, 768, true, MediaConstants.WebpExtension)) }
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
                                Url = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, newsFile)),
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
