using Foundation.Extensions.ModelBuilders;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Foundation.PageContent.ComponentModels;
using System.Threading.Tasks;
using Buyer.Web.Shared.ViewModels.Headers;
using System.Globalization;
using Buyer.Web.Areas.Home.ViewModel;

namespace Buyer.Web.Areas.Home.ModelBuilders
{
    public class HomePageModelBuilder : IAsyncComponentModelBuilder<ComponentModelBase, HomePageViewModel>
    {
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, HeroSliderViewModel> heroSliderModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, HomePageCarouselGridViewModel> carouselGridModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, HomePageContentGridViewModel> contentGridModelBuilder;
        private readonly IAsyncComponentModelBuilder<ComponentModelBase, HomePageNewsCarouselGridViewModel> newsModelBuilder;
        private readonly IModelBuilder<FooterViewModel> footerModelBuilder;

        public HomePageModelBuilder(
            IAsyncComponentModelBuilder<ComponentModelBase, BuyerHeaderViewModel> headerModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, MainNavigationViewModel> mainNavigationModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, HeroSliderViewModel> heroSliderModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, HomePageCarouselGridViewModel> carouselGridModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, HomePageContentGridViewModel> contentGridModelBuilder,
            IAsyncComponentModelBuilder<ComponentModelBase, HomePageNewsCarouselGridViewModel> newsModelBuilder,
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
                Locale = CultureInfo.CurrentUICulture.Name,
                Header = await headerModelBuilder.BuildModelAsync(componentModel),
                MainNavigation = await mainNavigationModelBuilder.BuildModelAsync(componentModel),
                HeroSlider = await heroSliderModelBuilder.BuildModelAsync(componentModel),
                CarouselGrid = await carouselGridModelBuilder.BuildModelAsync(componentModel),
                ContentGrid = await contentGridModelBuilder.BuildModelAsync(componentModel),
                NewsCarouselGrid = await newsModelBuilder.BuildModelAsync(componentModel),
                Footer = footerModelBuilder.BuildModel()
            };

            return viewModel;
        }
    }
}
