using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientAccountManagersPageViewModel : BasePageViewModel 
    {
        public CatalogViewModel<ClientAccountManager> Catalog { get; set; }
    }
}
