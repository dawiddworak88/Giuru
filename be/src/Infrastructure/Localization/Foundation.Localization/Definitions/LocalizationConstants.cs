using System;

namespace Foundation.Localization.Definitions
{
    public static class LocalizationConstants
    {
        public static readonly string CultureRouteConstraint = "cultureCode";
        public static readonly string YearToken = "[year]";
        public static readonly DateTimeOffset ExpirationDateOfLocalizationCookie = DateTimeOffset.UtcNow.AddYears(1);
    }
}
