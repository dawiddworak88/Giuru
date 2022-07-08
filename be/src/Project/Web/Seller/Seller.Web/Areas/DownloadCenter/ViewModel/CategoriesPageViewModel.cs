using Seller.Web.Areas.DownloadCenter.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.DownloadCenter.ViewModel
{
    public class CategoriesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<Category> Catalog { get; set; }
    }
}
