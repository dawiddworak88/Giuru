using Buyer.Web.Shared.ViewModels.Base;

namespace Buyer.Web.Areas.News.ViewModel
{
    public class NewsItemPageViewModel : BasePageViewModel
    {
        public NewsItemBreadcrumbsViewModel Breadcrumbs { get; set; }
        public NewsItemDetailsViewModel NewsItemDetails { get; set; }
    }
}
