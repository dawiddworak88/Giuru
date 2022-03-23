using Buyer.Web.Shared.ViewModels.Headers;
using Foundation.PageContent.Components.CarouselGrids.ViewModels;
using Foundation.PageContent.Components.ContentGrids.ViewModels;
using Foundation.PageContent.Components.Footers.ViewModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Foundation.PageContent.Components.MainNavigations.ViewModels;

namespace Buyer.Web.Areas.Home.ViewModel
{
    public class HomePageViewModel
    {
        public BuyerHeaderViewModel Header { get; set; }
        public MainNavigationViewModel MainNavigation { get; set; }
        public HeroSliderViewModel HeroSlider { get; set; }
        public CarouselGridViewModel CarouselGrid { get; set; }
        public ContentGridViewModel ContentGrid { get; set; }
        public NewsViewModel NewsCarousel { get; set; }
        public FooterViewModel Footer { get; set; }
    }
}
