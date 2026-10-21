using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class DiscountCodesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<DiscountCode> Catalog { get; set; }
    }
}
