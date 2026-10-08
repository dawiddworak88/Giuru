using System;
using System.Threading;
using System.Threading.Tasks;

namespace Foundation.Pricing.DiscountCodes
{
    /// <summary>
    /// The one policy deciding whether a discount code may be used for a client. It is shared by the pricing
    /// seam (the price client resolver decorator), the basket write seam and checkout, and it remembers its
    /// answer for the lifetime of the request so a basket save costs one lookup, not one per seam.
    /// </summary>
    public interface IDiscountCodeValidator
    {
        Task<DiscountCodeValidation> ValidateAsync(Guid? clientId, string discountCode, string token, CancellationToken cancellationToken = default);
    }
}
