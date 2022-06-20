using Buyer.Web.Areas.News.Definitions;
using Buyer.Web.Areas.News.DomainModels;
using Buyer.Web.Areas.News.ViewModel;
using Buyer.Web.Shared.Repositories.News;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.Media.Services.MediaServices;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.Images;
using Foundation.PageContent.Components.ListItems.ViewModels;
using Foundation.PageContent.Definitions;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.News.ModelBuilders
{
    public class NewsCatalogModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, NewsCatalogViewModel>
    {
        private readonly LinkGenerator linkGenerator;
        private readonly INewsRepository newsRepository;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;
        private readonly IStringLocalizer<GlobalResources> globalLocalizer;
        private readonly IMediaService mediaService;

        public NewsCatalogModelBuilder(
            IStringLocalizer<NewsResources> newsLocalizer,
            IStringLocalizer<GlobalResources> globalLocalizer,
            INewsRepository newsRepository,
            IMediaService mediaService,
            LinkGenerator linkGenerator)
        {
            this.linkGenerator = linkGenerator;
            this.newsLocalizer = newsLocalizer;
            this.globalLocalizer = globalLocalizer;
            this.newsRepository = newsRepository;
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
                        newsItem.ThumbImageUrl = this.mediaService.GetMediaUrl(newsItem.ThumbnailImageId.Value, 1024);
                        newsItem.ThumbImages = new List<SourceViewModel>
                        { 
                            new SourceViewModel { Media = MediaConstants.FullHdMediaQuery, Srcset = this.mediaService.GetMediaUrl(newsItem.ThumbnailImageId.Value, 1024) },
                            new SourceViewModel { Media = MediaConstants.DesktopMediaQuery, Srcset = this.mediaService.GetMediaUrl(newsItem.ThumbnailImageId.Value, 352) },
                            new SourceViewModel { Media = MediaConstants.TabletMediaQuery, Srcset = this.mediaService.GetMediaUrl(newsItem.ThumbnailImageId.Value, 608) },
                            new SourceViewModel { Media = MediaConstants.MobileMediaQuery, Srcset = this.mediaService.GetMediaUrl(newsItem.ThumbnailImageId.Value, 768) }
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
