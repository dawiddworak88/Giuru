using Buyer.Web.Areas.News.Repositories;
using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
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

        public NewsItemDetailsModelBuilder(
            IOptions<AppSettings> options,
            INewsRepository newsRepository,
            ICdnService cdnService,
            IMediaHelperService mediaService,
            LinkGenerator linkGenerator)
        {
            this.options = options;
            this.linkGenerator = linkGenerator;
            this.newsRepository = newsRepository;
            this.cdnService = cdnService;
            this.mediaService = mediaService;
        }

        public async Task<NewsItemDetailsViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsItemDetailsViewModel
            {

            };

            var existingNews = await this.newsRepository.GetNewsItemAsync(componentModel.Token, componentModel.Language, componentModel.Id);
            if (existingNews is not null)
            {
                viewModel.Id = existingNews.Id;
                viewModel.Title = existingNews.Title;
                viewModel.Content = existingNews.Content;
                viewModel.CreatedDate = existingNews.CreatedDate;
                viewModel.HeroImageUrl = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, existingNews.HeroImageId, 1024, 1024, true, MediaConstants.WebpExtension));
                viewModel.HeroImages = new List<SourceViewModel>
                {
                    new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, existingNews.HeroImageId, 1024, 1024, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, existingNews.HeroImageId, 352, 352, true,MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, existingNews.HeroImageId, 608, 608, true, MediaConstants.WebpExtension)) },
                    new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, existingNews.HeroImageId, 768, 768, true, MediaConstants.WebpExtension)) },
                };
            }

            return viewModel;
        }
    }
}
