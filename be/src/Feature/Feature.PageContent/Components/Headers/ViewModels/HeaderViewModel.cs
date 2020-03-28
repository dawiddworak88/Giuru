using Feature.PageContent.Components.LanguageSwitchers.ViewModels;
using Feature.PageContent.Components.Links.ViewModels;
using System.Collections.Generic;

namespace Feature.PageContent.Components.Headers.ViewModels
{
    public class HeaderViewModel
    {
        public LogoViewModel Logo { get; set; }
        public LinkViewModel LoginLink { get; set; }
        public LanguageSwitcherViewModel LanguageSwitcher { get; set; }
        public IEnumerable<LinkViewModel> Links { get; set; }
    }
}
