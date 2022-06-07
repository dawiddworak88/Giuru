using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientManagersPageViewModel : BasePageViewModel 
    {
        public CatalogViewModel<ClientManager> Catalog { get; set; }
    }
}
