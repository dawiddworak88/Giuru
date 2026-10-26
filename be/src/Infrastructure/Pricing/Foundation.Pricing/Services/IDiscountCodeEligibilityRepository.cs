using System;
using System.Threading.Tasks;

namespace Foundation.Pricing.Services
{
    /// <summary>
    /// The only question the web apps ask Client.Api about a Discount Code: is it a Valid Discount Code
    /// for this Client. Every call is live - there is no caching, so a revocation applies on the next request.
    /// </summary>
    public interface IDiscountCodeEligibilityRepository
    {
        /// <summary>
        /// Returns the code's stored spelling when it is a Valid Discount Code for <paramref name="clientId"/>,
        /// otherwise null. Callers cannot tell a missing code from an unassigned or inactive one.
        /// </summary>
        Task<string> GetValidDiscountCodeAsync(string token, string language, Guid? clientId, string discountCode);
    }
}
