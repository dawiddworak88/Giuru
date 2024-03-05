using System;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientNotificationTypeFormViewModel
    {
        public Guid? Id { get; set; }
        public string IdLabel { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string NameRequiredErrorMessage { get; set; }
        public string NameLabel { get; set; }
        public string SaveText { get; set; }
        public string SaveUrl { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string NavigateToClientNotificationTypes { get; set; }
        public string ClientNotificationTypesUrl { get; set; }
    }
}
