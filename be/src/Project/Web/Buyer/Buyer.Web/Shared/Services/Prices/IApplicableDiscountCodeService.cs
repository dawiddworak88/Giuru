using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Prices
{
    /// <summary>
    /// Decides whether a Discount Code may be used on this request. Buyers only ever act for the
    /// Client in their claims, so the Client is never passed in.
    /// </summary>
    public interface IApplicableDiscountCodeService
    {
        /// <summary>
        /// Returns the code's stored spelling when it is a Valid Discount Code for the signed-in Client,
        /// otherwise null. Always null while Grula is not configured, without calling Client.Api.
        /// </summary>
        Task<string> GetValidAsync(string token, string discountCode);
    }
}
