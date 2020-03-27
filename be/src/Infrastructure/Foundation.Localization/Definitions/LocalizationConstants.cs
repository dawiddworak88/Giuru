using System;

namespace Foundation.Localization.Definitions
{
    public static class LocalizationConstants
    {
        public const string CultureRouteConstraint = "cultureCode";

        public static readonly DateTimeOffset ExpirationDateOfLocalizationCookie = DateTimeOffset.UtcNow.AddYears(1);
    }
}
