using Foundation.Pricing.DiscountCodes;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Clients
{
    public interface IDiscountCodesRepository
    {
        /// <summary>
        /// Asks Client.Api whether a code can be applied for the signed-in buyer's own client. Throws when the call fails:
        /// a failed call is not an answer, so the caller must not read it as one.
        /// </summary>
        Task<DiscountCodeValidation> ValidateAsync(string token, string language, Guid clientId, string code, CancellationToken cancellationToken = default);
    }
}
