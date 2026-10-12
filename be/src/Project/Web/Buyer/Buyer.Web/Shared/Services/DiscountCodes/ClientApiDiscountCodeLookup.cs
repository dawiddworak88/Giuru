using Buyer.Web.Shared.Repositories.Clients;
using Foundation.Pricing.DiscountCodes;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Buyer.Web.Shared.Services.DiscountCodes
{
    /// <summary>
    /// The buyer app's port to Client.Api: one call with the buyer's own token. The client id is the buyer's own client,
    /// and Client.Api checks it against the organisation of the token. A failed call throws and is turned into "unverified"
    /// by <see cref="DiscountCodeValidator"/>; it is never read as an answer.
    /// </summary>
    public class ClientApiDiscountCodeLookup : IDiscountCodeLookup
    {
        private readonly IDiscountCodesRepository _discountCodesRepository;

        public ClientApiDiscountCodeLookup(IDiscountCodesRepository discountCodesRepository)
        {
            _discountCodesRepository = discountCodesRepository;
        }

        public Task<DiscountCodeValidation> ValidateAsync(Guid clientId, string discountCode, string token)
        {
            return _discountCodesRepository.ValidateAsync(token, CultureInfo.CurrentUICulture.Name, clientId, discountCode);
        }
    }
}
