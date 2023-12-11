using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientFieldsPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<ClientField> Catalog { get; set; }
    }
}
