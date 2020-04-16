using System.Globalization;

namespace Foundation.Localization.Services
{
    public class CultureService : ICultureService
    {
        public void SetCulture(string culture)
        {
            CultureInfo.CurrentCulture = new CultureInfo(culture);
            CultureInfo.CurrentUICulture = new CultureInfo(culture);
        }
    }
}
