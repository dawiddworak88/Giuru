using Foundation.PageContent.Components.Languages.ViewModels;
using System.Collections.Generic;

namespace Seller.Web.Areas.Clients.ViewModels
{
    public class ClientFormViewModel
    {
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
        public IEnumerable<LanguageViewModel> Languages { get; set; }
    }
}
