using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientApplicationsPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<ClientApplication> Catalog { get; set; }
    }
}
