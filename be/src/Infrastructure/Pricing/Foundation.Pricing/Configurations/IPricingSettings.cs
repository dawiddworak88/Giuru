namespace Foundation.Pricing.Configurations
{
    public interface IPricingSettings
    {
        string GrulaAccessToken { get; }

        string GrulaEnvironmentId { get; }

        string DefaultCurrency { get; }

        string EnablePricesForClients { get; }

        bool IsGrulaConfigured { get; }

        /// <summary>
        /// Rollout switch for discount code enforcement. It controls enforcement only, never whether discount codes
        /// can be managed. Use <see cref="PricingSettingsExtensions.IsDiscountCodeEnforced"/> to read it.
        /// </summary>
        bool DiscountCodeEnforcementEnabled { get; }
    }
}
