using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientsPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<Client> Catalog { get; set; }
    }
}