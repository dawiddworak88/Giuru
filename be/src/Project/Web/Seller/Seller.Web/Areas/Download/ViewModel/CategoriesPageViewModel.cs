using Seller.Web.Areas.Download.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Download.ViewModel
{
    public class CategoriesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<Category> Catalog { get; set; }
    }
}
