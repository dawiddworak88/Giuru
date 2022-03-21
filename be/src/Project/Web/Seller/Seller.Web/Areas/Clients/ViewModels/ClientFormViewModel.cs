using Foundation.PageContent.Components.Languages.ViewModels;
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
        public IEnumerable<LanguageViewModel> Languages { get; set; }
    }
}
