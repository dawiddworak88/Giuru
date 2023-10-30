using Foundation.PageContent.Components.ListItems.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientDeliveryAddressFormViewModel
    {
        public Guid? Id { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? CountryId { get; set; }
        public string Title { get; set; }
        public string Recipient { get; set; }
        public string RecipientLabel { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberLabel { get; set; }
        public string Street { get; set; }
        public string StreetLabel { get; set; }
        public string Region { get; set; }
        public string RegionLabel { get; set; }
        public string PostCode { get; set; }
        public string PostCodeLabel { get; set; }
        public string City { get; set; }
        public string CityLabel { get; set; }
        public string CountryLabel { get; set; }
        public string SaveText { get; set; }
        public string ClientLabel { get; set; }
        public string NavigateToClientDeliveryAddresses { get; set; }
        public string FieldRequiredErrorMessage { get; set; }
        public string GeneralErrorMessage { get; set; }
        public IEnumerator<ListItemViewModel> Clients { get; set; }
        public IEnumerable<ListItemViewModel> Countries { get; set; }
    }
}
