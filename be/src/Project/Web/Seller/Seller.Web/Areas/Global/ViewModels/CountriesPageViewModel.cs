using Seller.Web.Areas.Global.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Global.ViewModels
{
    public class CountriesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<Country> Catalog { get; set; }
    }
}
