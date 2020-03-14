using System.Collections.Generic;

namespace Feature.Localization.ViewModels
{
    public class LanguageSwitcherViewModel
    {
        public List<LanguageViewModel> AvailableLanguages { get; set; }
        public string SelectedLanguageUniqueId { get; set; }
    }
}
