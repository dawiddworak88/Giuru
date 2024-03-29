﻿using Buyer.Web.Shared.ViewModels.Analytics;
using Buyer.Web.Shared.ViewModels.Base;
using Foundation.PageContent.Components.CarouselGrids.ViewModels;
using Foundation.PageContent.Components.ContentGrids.ViewModels;
using Foundation.PageContent.Components.HeroSliders.ViewModels;
using Foundation.PageContent.Components.Metadatas.ViewModels;

namespace Buyer.Web.Areas.Home.ViewModel
{
    public class HomePageViewModel : BasePageViewModel
    {
        public MetadataViewModel Metadata { get; set; }
        public HeroSliderViewModel HeroSlider { get; set; }
        public OrdersAnalyticsDetailViewModel OrdersAnalytics { get; set; }
        public CarouselGridViewModel CarouselGrid { get; set; }
        public ContentGridViewModel ContentGrid { get; set; }
        public CarouselGridViewModel NewsCarouselGrid { get; set; }
    }
}
