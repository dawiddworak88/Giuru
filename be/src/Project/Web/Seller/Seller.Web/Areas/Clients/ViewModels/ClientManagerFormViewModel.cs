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
        public string IdLabel { get; set; }
        public string SaveText { get; set; }
        public string SaveUrl { get; set; }
        public string NavigateToManagersText { get; set; }
        public string ManagersUrl { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string EmailFormatErrorMessage { get; set; }
        public string FirstNameLabel { get; set; }
        public string FirstName { get; set; }
        public string LastNameLabel { get; set; }
        public string LastName { get; set; }
        public string PhoneNumberLabel { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailLabel { get; set; }
        public string Email { get; set; }
        public IEnumerable<ListItemViewModel> Clients { get; set; }
    }
}
