using System;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientGroupFormViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string IdLabel { get; set; }
        public string NameLabel { get; set; }
        public string Name { get; set; }
        public string GroupsUrl { get; set; }
        public string NavigateToGroupsText { get; set; }
        public string SaveText { get; set; }
        public string SaveUrl { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string NameRequiredErrorMessage { get; set; }
    }
}
