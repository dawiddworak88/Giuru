using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientApprovalsPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<ClientApproval> Catalog { get; set; }
    }
}
