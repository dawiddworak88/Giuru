using Seller.Web.Areas.News.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.News.ViewModel
{
    public class NewsPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<NewsItem> Catalog { get; set; }
    }
}
