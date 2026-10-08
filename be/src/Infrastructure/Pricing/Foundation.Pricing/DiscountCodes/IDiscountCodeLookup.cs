using System;
using System.Threading;
using System.Threading.Tasks;

namespace Foundation.Pricing.DiscountCodes
{
    /// <summary>
    /// Port implemented by each web app: one HTTP call to Client.Api that answers whether a code can be applied
    /// for a client. Client.Api returns an explicit string status that the adapter maps to
    /// <see cref="DiscountCodeValidationStatus"/>; the two services do not share an HTTP DTO.
    /// An implementation must not turn a failed call into an applicability answer: failures throw, or return
    /// <see cref="DiscountCodeValidationStatus.Unavailable"/>.
    /// </summary>
    public interface IDiscountCodeLookup
    {
        Task<DiscountCodeValidation> ValidateAsync(Guid clientId, string discountCode, string token, CancellationToken cancellationToken = default);
    }
}
