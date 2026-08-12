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
        InvalidPriceDrivers,
        ServiceUnavailable,
        MissingResponse
    }
}