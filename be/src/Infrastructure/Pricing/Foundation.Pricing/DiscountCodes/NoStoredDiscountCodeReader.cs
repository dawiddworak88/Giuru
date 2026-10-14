using System;
using System.Threading.Tasks;

namespace Foundation.Pricing.DiscountCodes
{
    /// <summary>
    /// For an app whose basket is not bound to the client it is priced for (the seller's). Its basket save validates a
    /// stored code for the client of every save, so no stored code is in force without validation, and a price read
    /// validates every code it is given in the same way.
    /// </summary>
    public sealed class NoStoredDiscountCodeReader : IStoredDiscountCodeReader
    {
        public Task<string> ReadAsync(Guid? clientId, string token)
        {
            return Task.FromResult<string>(null);
        }
    }
}
