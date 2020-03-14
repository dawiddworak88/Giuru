using System;

namespace Foundation.Localization.Definitions
{
    public class LocalizationConstants
    {
        public const string CultureRouteConstraint = "cultureCode";

        public static readonly DateTimeOffset ExpirationDateOfLocalizationCookie = DateTimeOffset.UtcNow.AddYears(1);
    }
}
