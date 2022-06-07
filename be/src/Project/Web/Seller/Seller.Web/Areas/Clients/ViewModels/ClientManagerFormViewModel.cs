using Foundation.PageContent.Components.ListItems.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientManagerFormViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public string ClientsLabel { get; set; }
        public string IdLabel { get; set; }
        public string NoClientsText { get; set; }
        public string SelectClients { get; set; }
        public string SaveText { get; set; }
        public string NavigateToManagersText { get; set; }
        public string ManagersUrl { get; set; }
        public string GeneralErrorMessage { get; set; }
        public IEnumerable<ListItemViewModel> Clients { get; set; }
    }
}
