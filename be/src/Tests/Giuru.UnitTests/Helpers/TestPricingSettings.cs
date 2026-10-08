using Foundation.Pricing.Configurations;
using System;

namespace Giuru.UnitTests.Helpers
{
    public sealed class TestPricingSettings : IPricingSettings
    {
        public string GrulaAccessToken { get; set; } = "test-token";
        public string GrulaEnvironmentId { get; set; } = "00000000-0000-0000-0000-000000000001";
        public string DefaultCurrency { get; set; }
        public string EnablePricesForClients { get; set; }
        public bool DiscountCodeEnforcementEnabled { get; set; }

        public bool IsGrulaConfigured =>
            !string.IsNullOrWhiteSpace(GrulaAccessToken) && Guid.TryParse(GrulaEnvironmentId, out _);
    }
}
