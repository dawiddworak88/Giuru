﻿using Foundation.PageContent.Components.Languages.ViewModels;
using Foundation.PageContent.Components.ListItems.ViewModels;
using System;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientFormViewModel
    {
        public string Title { get; set; }
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string CommunicationLanguage { get; set; }
        public string GeneralErrorMessage { get; set; }
        public string NameLabel { get; set; }
        public string EmailLabel { get; set; }
        public string LanguageLabel { get; set; }
        public string NameRequiredErrorMessage { get; set; }
        public string EmailRequiredErrorMessage { get; set; }
        public string LanguageRequiredErrorMessage { get; set; }
        public string EmailFormatErrorMessage { get; set; }
        public string ClientDetailText { get; set; }
        public string EnterNameText { get; set; }
        public string EnterEmailText { get; set; }
        public string SaveUrl { get; set; }
        public string SaveText { get; set; }
        public string AccountUrl { get; set; }
        public string AccountText { get; set; }
        public string ClientsUrl { get; set; }
        public string NavigateToClientsLabel { get; set; }
        public string IdLabel { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumberLabel { get; set; }
        public string ResetPasswordText { get; set; }
        public bool HasAccount { get; set; }
        public bool IsDisabled { get; set; }
        public string NoGroupsText { get; set; }
        public string GroupsLabel { get; set; }
        public string NoManagersText { get; set; }
        public string ClientManagerLabel { get; set; }
        public string CountryLabel { get; set; }
        public Guid? CountryId { get; set; }
        public string PreferedCurrencyLabel { get; set; }
        public Guid? PreferedCurrencyId { get; set; }
        public string ActiveLabel { get; set; }
        public string InActiveLabel { get; set; }
        public string DeliveryAddressLabel { get; set; }
        public string BillingAddressLabel { get; set; }
        public string ExpressedOnLabel { get; set; }
        public Guid? DefaultDeliveryAddressId { get; set; }
        public Guid? DefaultBillingAddressId { get; set; }
        public IEnumerable<Guid> ClientGroupsIds { get; set; }
        public IEnumerable<Guid> ClientManagersIds { get; set; }
        public IEnumerable<LanguageViewModel> Languages { get; set; }
        public IEnumerable<ListItemViewModel> ClientGroups { get; set; }
        public IEnumerable<ListItemViewModel> Countries { get; set; }
        public IEnumerable<ListItemViewModel> Currencies { get; set; }
        public IEnumerable<ListItemViewModel> ClientAddresses { get; set; }
        public IEnumerable<ClientAccountManagerViewModel> ClientManagers { get; set; }
        public IEnumerable<ClientFieldViewModel> ClientFields { get; set; }
        public IEnumerable<ApprovalViewModel> ClientApprovals { get; set; }
    }
}
