namespace Foundation.Pricing.Configurations
{
    public interface IPricingSettings
    {
        string GrulaAccessToken { get; }

        string GrulaEnvironmentId { get; }

        string DefaultCurrency { get; }

        string EnablePricesForClients { get; }

        bool IsGrulaConfigured { get; }
    }
}
