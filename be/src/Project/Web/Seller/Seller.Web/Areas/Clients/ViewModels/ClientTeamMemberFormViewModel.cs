using System;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientTeamMemberFormViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public string EmailFormatErrorMessage { get; set; }
        public string SaveText { get; set; }
        public string FirstName { get; set; }
        public string FirstNameLabel { get; set; }
        public string LastName { get; set; }
        public string LastNameLabel { get; set; }
        public string Email { get; set; }
        public string EmailLabel { get; set; }
        public string IdLabel { get; set; }
        public bool IsDisabled { get; set; }
        public string ActiveLabel { get; set; }
        public string InActiveLabel { get; set; }
        public string NavigateToClientTeamMembersListText { get; set; }
        public string ClientTeamMembersUrl { get; set; }
        public string SaveUrl { get; set; }
    }
}
