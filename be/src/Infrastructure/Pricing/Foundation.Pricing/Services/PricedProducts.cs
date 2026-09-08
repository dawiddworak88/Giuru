using Foundation.Pricing.DomainModels;
using System.Collections.Generic;

namespace Foundation.Pricing.Services
{
    /// <summary>
    /// Positional pricing result for a catalog batch. Wraps the response so callers cannot read
    /// past a short Grula response by index.
    /// </summary>
    public readonly struct PricedProducts
    {
        public static readonly PricedProducts Empty = default;

        private readonly IReadOnlyList<Price> _prices;

        public PricedProducts(IReadOnlyList<Price> prices) => _prices = prices;

        public bool IsEmpty => _prices is null || _prices.Count is 0;

        /// <summary>Positional lookup that cannot throw and cannot read past a short Grula response.</summary>
        public Price ElementAtOrDefault(int index)
            => _prices is null || index < 0 || index >= _prices.Count ? null : _prices[index];
    }
}
