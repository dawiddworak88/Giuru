using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientAddressesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<ClientAddress> Catalog { get; set; }
    }
}
