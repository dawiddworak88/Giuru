using System.Collections.Generic;

namespace Feature.PageContent.Shared.LanguageSwitchers.ViewModels
{
    public class LanguageSwitcherViewModel
    {
        public List<LanguageViewModel> AvailableLanguages { get; set; }
        public string SelectedLanguageText { get; set; }
    }
}
