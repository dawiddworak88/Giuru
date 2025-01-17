using Foundation.PageContent.Components.Languages.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientApplicationFormViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string IdLabel { get; set; }
        public string CompanyNameLabel { get; set; }
        public string CompanyName { get; set; }
        public string FirstNameLabel { get; set; }
        public string FirstName { get; set; }
        public string LastNameLabel { get; set; }
        public string LastName { get; set; }
        public string ContactJobTitleLabel { get; set; }
        public string ContactJobTitle { get; set; }
        public string EmailLabel { get; set; }
        public string Email { get; set; }
        public string PhoneNumberLabel { get; set; }
        public string PhoneNumber { get; set; }
        public string SaveText { get; set; }
        public string SaveUrl { get; set; }
        public string BackToClientsApplicationsText { get; set; }
        public string ClientsApplicationsUrl { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public string EmailFormatErrorMessage { get; set; }
        public string SelectJobTitle { get; set; }
        public string LanguageLabel { get; set; }
        public string CommunicationLanguage { get; set; }
        public string AddressFullNameLabel { get; set; }
        public string AddressPhoneNumberLabel { get; set; }
        public string AddressStreetLabel { get; set; }
        public string AddressRegionLabel { get; set; }
        public string AddressPostalCodeLabel { get; set; }
        public string AddressCityLabel { get; set; }
        public string AddressCountryLabel { get; set; }
        public string DeliveryAddressEqualBillingAddressText { get; set; }
        public bool IsDeliveryAddressEqualBillingAddress { get; set; }
        public string BillingAddressTitle { get; set; }
        public ClientApplicationAddressViewModel BillingAddress { get; set; }
        public string DeliveryAddressTitle { get; set; }
        public ClientApplicationAddressViewModel DeliveryAddress { get; set; }
        public IEnumerable<ContactJobTitle> ContactJobTitles { get; set; }
        public IEnumerable<LanguageViewModel> Languages { get; set; }
    }
}
