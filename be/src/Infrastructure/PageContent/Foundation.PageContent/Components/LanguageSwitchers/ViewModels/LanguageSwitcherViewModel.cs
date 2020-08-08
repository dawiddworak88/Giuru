using System.Collections.Generic;

namespace Foundation.PageContent.Components.LanguageSwitchers.ViewModels
{
    public class LanguageSwitcherViewModel
    {
        public List<LanguageViewModel> AvailableLanguages { get; set; }
        public string SelectedLanguageUrl { get; set; }
    }
}
