namespace Foundation.Pricing.Configurations
{
    public static class PricingSettingsExtensions
    {
        /// <summary>
        /// Whether a discount code must be verified before it is used. Every seam - pricing, basket writes, checkout
        /// and the optional load warnings - uses this one condition so that they switch together.
        /// </summary>
        public static bool IsDiscountCodeEnforced(this IPricingSettings settings)
        {
            return settings.IsGrulaConfigured && settings.DiscountCodeEnforcementEnabled;
        }
    }
}
