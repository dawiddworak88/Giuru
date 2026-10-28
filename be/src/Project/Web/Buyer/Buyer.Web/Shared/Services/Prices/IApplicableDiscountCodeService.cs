using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.Prices
{
    /// <summary>
    /// Decides which Discount Code a read - a price lookup or a page showing the Applied Discount Code -
    /// may use. Buyers only ever act for the Client in their claims, so the Client is never passed in.
    /// Checkout must not use it: a code that cannot be checked there has to fail, not be dropped.
    /// </summary>
    public interface IApplicableDiscountCodeService
    {
        /// <summary>
        /// Returns the code's stored spelling when it is a Valid Discount Code for the signed-in Client,
        /// otherwise null. Always null while Grula is not configured, without calling Client.Api, and
        /// null when Client.Api cannot answer, so browsing never breaks.
        /// </summary>
        Task<string> GetApplicableAsync(string token, string discountCode);
    }
}
