using Foundation.PageContent.Components.ListItems.ViewModels;
using System;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientListItemViewModel : ListItemViewModel
    {
        public Guid? DefaultDeliveryAddressId { get; set; }
    }
}
