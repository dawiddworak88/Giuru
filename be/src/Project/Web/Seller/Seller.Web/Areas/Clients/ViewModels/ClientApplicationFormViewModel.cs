using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientApplicationFormViewModel
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string IdLabel { get; set; }
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
        public string CompanyNameLabel { get; set; }
        public string CompanyName { get; set; }
        public string AddressLabel { get; set; }
        public string CompanyAddress { get; set; }
        public string CountryLabel { get; set; }
        public string CompanyCountry { get; set; }
        public string CityLabel { get; set; }
        public string CompanyCity { get; set; }
        public string PostalCodeLabel { get; set; }
        public string CompanyPostalCode { get; set; }
        public string RegionLabel { get; set; }
        public string CompanyRegion { get; set; }
        public string SaveText { get; set; }
        public string SaveUrl { get; set; }
        public string BackToClientsApplicationsText { get; set; }
        public string ClientsApplicationsUrl { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public string EmailFormatErrorMessage { get; set; }
        public string SelectJobTitle { get; set; }
        public IEnumerable<ContactJobTitle> ContactJobTitles { get; set; }
    }
}
