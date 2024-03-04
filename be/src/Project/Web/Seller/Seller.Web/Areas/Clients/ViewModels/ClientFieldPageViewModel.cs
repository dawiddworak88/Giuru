using Seller.Web.Areas.Clients.DomainModels;
using Seller.Web.Shared.ViewModels;
using System;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientFieldPageViewModel : BasePageViewModel
    {
        public Guid? Id { get; set; }
        public string FieldType { get; set; }
        public ClientFieldFormViewModel ClientFieldForm { get; set; }
        public CatalogViewModel<ClientFieldOption> Catalog { get; set; }
    }
}
