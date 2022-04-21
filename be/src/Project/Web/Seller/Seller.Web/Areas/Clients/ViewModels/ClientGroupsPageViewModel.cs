using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientGroupsPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<ClientGroup> Catalog { get; set; }
    }
}
