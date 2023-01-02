using System;

namespace Buyer.Web.Areas.Dashboard.ViewModel
{
    public class SettingsFormViewModel
    {
        public string GenerateAppSecretUrl { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string CopyLabel { get; set; }
        public string NameLabel { get; set; }
        public string Name { get; set; }
        public string AccountSettings { get; set; }
        public string AccountData { get; set; }
        public string EmailLabel { get; set; }
        public string PhoneNumberLabel { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string ApiIdentifierTitle { get; set; }
        public string ApiIdentifierDescription { get; set; }
        public string AppSecretLabel { get; set; }
        public string OrganisationLabel { get; set; }
        public string GenerateText { get; set; }
        public Guid? AppSecret { get; set; }
        public Guid? OrganisationId { get; set; }
    }
}
