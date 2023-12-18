using Seller.Web.Areas.Global.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Global.ViewModels
{
    public class CurrenciesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<Currency> Catalog { get; set; }
    }
}
