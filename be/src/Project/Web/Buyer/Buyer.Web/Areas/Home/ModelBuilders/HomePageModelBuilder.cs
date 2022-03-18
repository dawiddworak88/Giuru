using Buyer.Web.Areas.Home.ViewModel;
using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using Buyer.Web.Shared.ViewModels.Headers;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HomePageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HomePageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, HeroSliderViewModel> heroSliderModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, HomePageCarouselGridViewModel> carouselGridModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, HomePageContentGridViewModel> contentGridModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, NewsViewModel> newsModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public HomePageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, HeroSliderViewModel> heroSliderModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, HomePageCarouselGridViewModel> carouselGridModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, HomePageContentGridViewModel> contentGridModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, NewsViewModel> newsModelBuilder,
            IModelBuilder<FooterViewModel> footerModelBuilder)
        {
            this.headerModelBuilder = headerModelBuilder;
            this.mainNavigationModelBuilder = mainNavigationModelBuilder;
            this.heroSliderModelBuilder = heroSliderModelBuilder;
            this.carouselGridModelBuilder = carouselGridModelBuilder;
            this.contentGridModelBuilder = contentGridModelBuilder;
            this.footerModelBuilder = footerModelBuilder;
            this.newsModelBuilder = newsModelBuilder;
        }

        public async Task<HomePageViewModel> BuildModelAsync(ComponentModelBase componentModel)
        {

            var viewModel = new HomePageViewModel
            {
                Header = await this.headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await this.mainNavigationModelBuilder.BuildModelAsync(componentModel),
                HeroSlider = await this.heroSliderModelBuilder.BuildModelAsync(componentModel),
                CarouselGrid = await this.carouselGridModelBuilder.BuildModelAsync(componentModel),
                ContentGrid = await this.contentGridModelBuilder.BuildModelAsync(componentModel),
                NewsCarousel = await this.newsModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
