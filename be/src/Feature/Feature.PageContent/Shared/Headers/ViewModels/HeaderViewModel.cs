using Feature.PageContent.Shared.LanguageSwitchers.ViewModels;
using Feature.PageContent.Shared.Links.ViewModels;
using System.Collections.Generic;

namespace Feature.PageContent.Shared.Headers.ViewModels
{
    public class HeaderViewModel
    {
        public LogoViewModel Logo { get; set; }
        public LinkViewModel LoginLink { get; set; }
        public LanguageSwitcherViewModel LanguageSwitcher { get; set; }
        public IEnumerable<LinkViewModel> Links { get; set; }
    }
}
