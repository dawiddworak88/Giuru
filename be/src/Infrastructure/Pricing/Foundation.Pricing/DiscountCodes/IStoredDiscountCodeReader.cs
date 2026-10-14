using System;
using System.Threading.Tasks;

namespace Foundation.Pricing.DiscountCodes
{
    /// <summary>
    /// Port implemented by each web app: reads the discount code stored on the basket the current request prices for, when
    /// the app keeps a stored code in force without revalidating it. The fresh product pricing seam uses it to treat that
    /// code exactly as the basket save does, so a product page or catalog shows the price the basket will charge for it.
    /// An app that revalidates stored codes on every save registers <see cref="NoStoredDiscountCodeReader"/>.
    /// A failed read may throw: <see cref="VerifiedDiscountCodePriceClientResolver"/> then validates the code instead.
    /// </summary>
    public interface IStoredDiscountCodeReader
    {
        /// <returns>The stored code only when its prior validation is bound to this client and basket; otherwise <c>null</c>.</returns>
        Task<string> ReadAsync(Guid? clientId, string token);
    }
}
