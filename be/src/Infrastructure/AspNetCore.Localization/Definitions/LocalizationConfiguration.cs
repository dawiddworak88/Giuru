using System.Collections.Generic;

namespace AspNetCore.Localization.Definitions
{
    public class LocalizationConfiguration
    {
        public string DefaultRequestCulture { get; set; }
        public IEnumerable<string> SupportedCultures { get; set; }
    }
}
