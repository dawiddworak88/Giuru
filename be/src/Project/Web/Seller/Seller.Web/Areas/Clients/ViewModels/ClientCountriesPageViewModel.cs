using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientCountriesPageViewModel : BasePageViewModel
    {
        public CatalogViewModel<ClientCountry> Catalog { get; set; }
    }
}
