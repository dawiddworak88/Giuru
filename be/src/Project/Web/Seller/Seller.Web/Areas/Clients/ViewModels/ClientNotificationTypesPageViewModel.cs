using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientNotificationTypesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<ClientNotificationType> Catalog { get; set; }
    }
}
