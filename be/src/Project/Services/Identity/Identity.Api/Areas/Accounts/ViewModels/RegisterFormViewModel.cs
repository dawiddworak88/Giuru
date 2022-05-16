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
        public string CompanyAddressLabel { get; set; }
        public string CompanyCityLabel { get; set; }
        public string CompanyRegionLabel { get; set; }
        public string CompanyCountryLabel { get; set; }
        public string CompanyPostalCodeLabel { get; set; }
        public string AddressLabel { get; set; }
        public string ContactJobTitleLabel { get; set; }
        public string CityLabel { get; set; }
        public string RegionLabel { get; set; }
        public string CountryLabel { get; set; }
        public string YesLabel { get; set; }
        public string NoLabel { get; set; }
        public string ContactJobTitle { get; set; }
        public string ContactInformationTitle { get; set; }
        public string BusinessInformationTitle { get; set; }
        public string LogisticalInformationTitle { get; set; }
        public string EmailFormatErrorMessage { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public string SaveUrl { get; set; }
        public string OnlineRetailersLabel { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string AcceptReturnsLabel { get; set; }
        public string DirectlyShipLabel { get; set; }
        public string SaveText { get; set; }
        public IEnumerable<ContactJobTitle> ContactJobTitles { get; set; }
        public IEnumerable<StepViewModel> Steps { get; set; }
    }
}
