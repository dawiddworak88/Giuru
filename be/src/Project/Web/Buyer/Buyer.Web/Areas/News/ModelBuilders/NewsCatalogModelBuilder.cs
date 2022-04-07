using Buyer.Web.Areas.News.Definitions;
using Buyer.Web.Areas.News.DomainModels;
using Buyer.Web.Areas.News.Repositories.Categories;
using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Shared.Configurations;
using Buyer.Web.Shared.Repositories.News;
using Buyer.Web.Shared.Services.ContentDeliveryNetworks;
using Foundation.Extensions.ModelBuilders;
using Foundation.Extensions.Services.MediaServices;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ModelBuilders
{
    public class NewsCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsCatalogViewModel>
    {
        private readonly IOptions<AppSettings> options;
        private readonly LinkGenerator linkGenerator;
        private readonly INewsRepository newsRepository;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly ICdnService cdnService;
        private readonly IMediaHelperService mediaService;

        public NewsCatalogModelBuilder(
            IOptions<AppSettings> options,
            IStringLocalizer<NewsResources> newsLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            INewsRepository newsRepository,
            ICdnService cdnService,
            IMediaHelperService mediaService,
            LinkGenerator linkGenerator)
        {
            this.options = options;
            this.linkGenerator = linkGenerator;
            this.newsLocalizer = newsLocalizer;
            this.globalLocalizer = globalLocalizer;
            this.newsRepository = newsRepository;
            this.cdnService = cdnService;
            this.mediaService = mediaService;
        }

        public async Task<NewsCatalogViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new NewsCatalogViewModel
            {
                NewsApiUrl = this.linkGenerator.GetPathByAction("Get", "NewsApi", new { Area = "News", culture = CultureInfo.CurrentUICulture.Name }),
                AllCategoryLabel = this.newsLocalizer.GetString("AllCategoryLabel"),
                NoResultsLabel = this.globalLocalizer.GetString("NoResultsLabel"),
                TopUpContentSize = NewsConstants.DefaultPageSize
            };

            var news = await this.newsRepository.GetNewsItemsAsync(componentModel.Token, componentModel.Language, NewsConstants.DefaultPageIndex, NewsConstants.DefaultPageSize, null, $"{nameof(NewsItem.CreatedDate)} desc");
            if (news is not null)
            {
                var categories = new List<ListItemViewModel>();
                foreach(var newsItem in news.Data)
                {
                    var existingCategory = categories.FirstOrDefault(x => x.Name == newsItem.CategoryName);
                    if (existingCategory is null)
                    {
                        var category = new ListItemViewModel
                        {
                            Id = newsItem.CategoryId,
                            Name = newsItem.CategoryName
                        };

                        categories.Add(category);
                    }

                    if (newsItem.ThumbnailImageId.HasValue)
                    {
                        newsItem.ThumbImageUrl = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, newsItem.ThumbnailImageId.Value, 1024, 1024, true, MediaConstants.WebpExtension));
                        newsItem.ThumbImages = new List<SourceViewModel>
                        { 
                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, newsItem.ThumbnailImageId.Value, 1024, 1024, true, MediaConstants.WebpExtension)) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, newsItem.ThumbnailImageId.Value, 352, 352, true,MediaConstants.WebpExtension)) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, newsItem.ThumbnailImageId.Value, 608, 608, true, MediaConstants.WebpExtension)) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.cdnService.GetCdnUrl(this.mediaService.GetFileUrl(this.options.Value.MediaUrl, newsItem.ThumbnailImageId.Value, 768, 768, true, MediaConstants.WebpExtension)) }
                        };
                    }
                };

                if (news.Total >= (NewsConstants.DefaultPageSize + 1))
                {
                    viewModel.HasMore = true;
                }

                viewModel.Categories = categories.OrderBy(x => x.Name);
                viewModel.PagedResults = news;
            }

            return viewModel;
        }
    }
}
