using Seller.Web.Areas.News.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.News.ViewModel
{
    public class CategoriesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<Category> Catalog { get; set; }
    }
}
