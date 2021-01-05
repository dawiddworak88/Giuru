using Foundation.PageContent.Components.ListItems.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Orders.ViewModel
{
    public class OrderFormViewModel
    {
        public string Title { get; set; }
        public Guid? Id { get; set; }
        public string SelectClientLabel { get; set; }
        public string ClientRequiredErrorMessage { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string SaveText { get; set; }
        public string SaveUrl { get; set; }
        public IEnumerable<ListItemViewModel> Clients { get; set; }
    }
}
