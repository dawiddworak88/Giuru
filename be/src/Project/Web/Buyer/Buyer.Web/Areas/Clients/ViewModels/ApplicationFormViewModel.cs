using Foundation.PageContent.Components.Languages.ViewModels;
using System.Collections.Generic;

namespace Buyer.Web.Areas.Clients.ViewModels
{
    public class ApplicationFormViewModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string FirstNameLabel { get; set; }
        public string LastNameLabel { get; set; }
        public string EmailLabel { get; set; }
        public string PhoneNumberLabel { get; set; }
        public string LanguageLabel { get; set; }
        public string CompanyNameLabel { get; set; }
        public string ContactJobTitleLabel { get; set; }
        public string BillingAddressTitle { get; set; }
        public string DeliveryAddressTitle { get; set; }
        public string AddressFullNameLabel { get; set; }
        public string AddressPhoneNumberLabel { get; set; }
        public string AddressStreetLabel { get; set; }
        public string AddressRegionLabel { get; set; }
        public string AddressPostalCodeLabel { get; set; }
        public string AddressCityLabel { get; set; }
        public string AddressCountryLabel { get; set; }
        public string YesLabel { get; set; }
        public string NoLabel { get; set; }
        public string ContactInformationTitle { get; set; }
        public string EmailFormatErrorMessage { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public string SaveUrl { get; set; }
        public string OnlineRetailersLabel { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string AcceptReturnsLabel { get; set; }
        public string SaveText { get; set; }
        public string SelectJobTitle { get; set; }
        public string SignInUrl { get; set; }
        public string AcceptTermsText { get; set; }
        public string DeliveryAddressEqualBillingAddressText { get; set; }
        public IEnumerable<ContactJobTitle> ContactJobTitles { get; set; }
        public IEnumerable<StepViewModel> Steps { get; set; }
        public IEnumerable<LanguageViewModel> Languages { get; set; }
        public string PrivacyPolicyUrl { get; set; }
        public string RegulationsUrl { get; set; }
        public string PrivacyPolicy { get; set; }
        public string Regulations { get; set; }
    }
}
