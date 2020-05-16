using System.Collections.Generic;

namespace Feature.PageContent.Components.LanguageSwitchers.ViewModels
{
    public class LanguageSwitcherViewModel
    {
        public List<LanguageViewModel> AvailableLanguages { get; set; }
        public string SelectedLanguageUrl { get; set; }
    }
}
