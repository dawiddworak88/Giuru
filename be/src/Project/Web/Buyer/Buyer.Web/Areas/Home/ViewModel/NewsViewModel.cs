using Foundation.PageContent.Components.CarouselGrids.ViewModels;

namespace Buyer.Web.Areas.Home.ViewModel
{
    public class NewsViewModel : CarouselGridViewModel
    {
        public string Title { get; set; }
        public bool IsNews { get; set; }
    }
}
