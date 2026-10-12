using System;
using System.Threading.Tasks;

namespace Foundation.Pricing.DiscountCodes
{
    /// <summary>
    /// The one policy deciding whether a discount code may be newly applied for a client. It is shared by the
    /// fresh product pricing seam (the price client resolver decorator) and the basket write seam, and it remembers
    /// its answer for the lifetime of the request so a basket save costs one lookup, not one per seam.
    /// </summary>
    public interface IDiscountCodeValidator
    {
        Task<DiscountCodeValidation> ValidateAsync(Guid? clientId, string discountCode, string token);
    }
}
