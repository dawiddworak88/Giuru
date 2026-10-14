using System;

namespace Buyer.Web.Shared.Services.DiscountCodes
{
    public interface IBasketDiscountCodeBinding
    {
        bool IsBoundToClient(Guid? basketId, Guid? clientId, string discountCode);

        /// <summary>
        /// Remembers a successfully validated and saved code for this basket and client. A null code clears the binding.
        /// Do not bind a retained code unless its existing binding was verified or it was validated again.
        /// </summary>
        void Bind(Guid? basketId, Guid? clientId, string discountCode);
    }
}
