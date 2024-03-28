using Foundation.PageContent.Components.Headers.ViewModels;
using Foundation.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using System.Collections.Generic;

namespace Buyer.Web.Shared.ViewModels.Headers.SidebarMobile
{
    public class SidebarMobileViewModel
    {
        public LogoViewModel Logo { get; set; }
        public bool IsLoggedIn { get; set; }
        public LinkViewModel SignOutLink { get; set; }
        public LinkViewModel SignInLink { get; set; }
        public LanguageSwitcherViewModel LanguageSwitcher { get; set; }
        public IEnumerable<LinkViewModel> Links { get; set; }
    }
}
