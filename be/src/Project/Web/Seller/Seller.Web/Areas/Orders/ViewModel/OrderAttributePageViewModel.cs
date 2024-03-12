using Seller.Web.Areas.Orders.DomainModels;
using Seller.Web.Shared.ViewModels;
using System;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class OrderAttributePageViewModel : BasePageViewModel
    {
        public Guid? Id { get; set; }
        public string Type { get; set; }
        public OrderAttributeFormViewModel OrderAttributeForm { get; set; }
        public CatalogViewModel<OrderAttributeOption> Catalog { get; set; }
    }
}
