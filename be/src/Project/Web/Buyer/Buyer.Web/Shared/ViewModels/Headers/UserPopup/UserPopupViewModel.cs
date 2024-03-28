using Foundation.PageContent.Components.Links.ViewModels;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Headers.UserPopup
{
    public class UserPopupViewModel
    {
        public string WelcomeText { get; set; }
        public string Name { get; set; }
        public bool IsLoggedIn { get; set; }
        public LinkViewModel SignOutLink { get; set; }
        public LinkViewModel SignInLink { get; set; }
        public IEnumerable<LinkViewModel> Actions { get; set; }
    }
}
