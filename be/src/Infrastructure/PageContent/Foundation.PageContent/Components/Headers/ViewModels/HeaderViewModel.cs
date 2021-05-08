using Foundation.PageContent.Components.DrawerMenu.ViewModels;
using Foundation.PageContent.Components.LanguageSwitchers.ViewModels;
using Foundation.PageContent.Components.Links.ViewModels;
using System.Collections.Generic;

namespace Foundation.PageContent.Components.Headers.ViewModels
{
    public class HeaderViewModel
    {
        public bool IsLoggedIn { get; set; }
        public LinkViewModel SignOutLink { get; set; }
        public string DrawerBackLabel { get; set; }
        public string DrawerBackIcon { get; set; }
        public LogoViewModel Logo { get; set; }
        public IEnumerable<DrawerMenuViewModel> DrawerMenuCategories { get; set; }
        public LanguageSwitcherViewModel LanguageSwitcher { get; set; }
        public IEnumerable<LinkViewModel> Links { get; set; }
    }
}
