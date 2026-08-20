namespace Buyer.Web.Shared.DomainModels.Prices
{
    /// <summary>
    /// The outcome for one requested basket price. A missing Price is authoritative only
    /// when the pricing service successfully returned an entry without an amount.
    /// </summary>
    public sealed class PriceLookupResult
    {
        public PriceLookupStatus Status { get; init; }

        public Price Price { get; init; }
    }

    public enum PriceLookupStatus
    {
        Priced,
        AuthoritativeNoPrice,
        /// <summary>
        /// The price drivers for this line are incomplete (no primary SKU or no price-group
        /// attribute), so Grula was never asked. The line is persisted unpriced - this is a
        /// catalogue data-quality issue and must not fail the basket.
        /// </summary>
        InvalidPriceDrivers,
        ServiceUnavailable,
        MissingResponse,
        /// <summary>The client is outside EnablePricesForClients, so no price may be produced or stored.</summary>
        PricesHidden
    }
}