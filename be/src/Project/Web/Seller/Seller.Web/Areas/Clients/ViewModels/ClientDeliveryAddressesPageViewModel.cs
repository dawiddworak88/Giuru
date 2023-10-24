using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientDeliveryAddressesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<ClientDeliveryAddress> Catalog { get; set; }
    }
}
