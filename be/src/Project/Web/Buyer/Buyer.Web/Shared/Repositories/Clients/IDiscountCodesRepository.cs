using Foundation.Pricing.DiscountCodes;
using System;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Repositories.Clients
{
    /// <summary>Buyers never manage discount codes. They can only ask whether one may be applied for their client.</summary>
    public interface IDiscountCodesRepository
    {
        Task<DiscountCodeValidation> ValidateAsync(string token, string language, Guid clientId, string code);
    }
}
