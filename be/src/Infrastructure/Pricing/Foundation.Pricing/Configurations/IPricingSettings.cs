namespace Foundation.Pricing.Configurations
{
    public interface IPricingSettings
    {
        string GrulaAccessToken { get; }

        string GrulaEnvironmentId { get; }

        string DefaultCurrency { get; }

        string EnablePricesForClients { get; }

        bool IsGrulaConfigured { get; }

        /// <summary>The base address of Client.Api, which validates discount codes.</summary>
        string ClientUrl { get; }
    }
}
