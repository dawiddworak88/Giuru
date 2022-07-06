using Buyer.Web.Areas.Home.Definitions;
using Buyer.Web.Areas.Home.DomainModels;
using Buyer.Web.Areas.Home.ViewModel.Home;
using Buyer.Web.Shared.Repositories.News;
using Foundation.Extensions.ModelBuilders;
using Foundation.Localization;
using Foundation.PageContent.ComponentModels;
using Foundation.PageContent.Components.CarouselGrids.ViewModels;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Buyer.Web.Areas.Home.ModelBuilders.Home
{
    public class HomePageNewsModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HomePageNewsCarouselGridViewModel>
    {
        private readonly INewsRepository newsRepository;
        private readonly IStringLocalizer<NewsResources> newsLocalizer;

        public HomePageNewsModelBuilder(
            INewsRepository newsRepository,
            IStringLocalizer<NewsResources> newsLocalizer)
        {
            this.newsRepository = newsRepository;
            this.newsLocalizer = newsLocalizer;
        }

        public async Task<HomePageNewsCarouselGridViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {
            var viewModel = new HomePageNewsCarouselGridViewModel
            {
                Title = this.newsLocalizer.GetString("News")
            };

            var items = new List<CarouselGridItemViewModel>();

            var news = await this.newsRepository.GetNewsItemsAsync(
                componentModel.Token, componentModel.Language, Foundation.GenericRepository.Definitions.Constants.DefaultPageIndex, Foundation.GenericRepository.Definitions.Constants.DefaultItemsPerPage, null, $"{nameof(NewsItem.CreatedDate)} desc");

            if (news is not null && news.Total > 0)
            {
                var contentGridCarouselItems = new List<CarouselGridCarouselItemViewModel>();

                foreach (var newsItem in news.Data)
                {
                    var carouselItem = new CarouselGridCarouselItemViewModel
                    {
                        Id = newsItem.Id,
                        Title = newsItem.Title,
                        CategoryName = newsItem.CategoryName,
                        Subtitle = newsItem.Description,
                        Url = newsItem.Url,
                        ImageUrl = newsItem.ThumbImageUrl,
                        Sources = newsItem.ThumbImages,
                        CreatedDate = newsItem.CreatedDate
                    };

                    contentGridCarouselItems.Add(carouselItem);
                }

                items.Add(new CarouselGridItemViewModel { Id = HomeConstants.News.NewsId, Title = this.newsLocalizer.GetString("News"), CarouselItems = contentGridCarouselItems });

                viewModel.Items = items;
            }

            return viewModel;
        }
    }
}
