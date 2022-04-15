using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class GroupsPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<Groups> Catalog { get; set; }
    }
}
