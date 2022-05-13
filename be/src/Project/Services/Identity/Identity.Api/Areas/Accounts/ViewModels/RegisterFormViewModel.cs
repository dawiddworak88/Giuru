using System.Collections.Generic;

namespace Identity.Api.Areas.Accounts.ViewModels
{
    public class RegisterFormViewModel
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string FirstNameLabel { get; set; }
        public string LastNameLabel { get; set; }
        public string EmailLabel { get; set; }
        public string PhoneNumberLabel { get; set; }
        public string CompanyNameLabel { get; set; }
        public string AddressLabel { get; set; }
        public string ContactJobTitleLabel { get; set; }
        public string CityLabel { get; set; }
        public string RegionLabel { get; set; }
        public string PostalCodeLabel { get; set; }
        public string CountryLabel { get; set; }
        public string YesLabel { get; set; }
        public string NoLabel { get; set; }
        public string ContactJobTitle { get; set; }
        public string ContactInformationTitle { get; set; }
        public string BusinessInformationTitle { get; set; }
        public string LogisticalInformationTitle { get; set; }
        public string FirstNameRequiredErrorMessage { get; set; }
        public string LastNameRequiredErrorMessage { get; set; }
        public string NameRequiredErrorMessage { get; set; }
        public string EmailRequiredErrorMessage { get; set; }
        public string EmailFormatErrorMessage { get; set; }
        public string PhoneRequiredErrorMessage { get; set; }
        public string ContactJobTitleRequiredErrorMessage { get; set; }
        public string AddressRequiredErrorMessage { get; set; }
        public string CountryRequiredErrorMessage { get; set; }
        public string CityRequiredErrorMessage { get; set; }
        public string RegionRequiredErrorMessage { get; set; }
        public string PostalCodeRequiredErrorMessage { get; set; }
        public string LegallyRegisteredRequiredErrorMessage { get; set; }
        public IEnumerable<StepViewModel> Steps { get; set; }
    }
}
