using Seller.Web.Shared.DomainModels.Clients;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientGroupsPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<ClientGroup> Catalog { get; set; }
    }
}
