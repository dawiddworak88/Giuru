using Foundation.Pricing.DomainModels;
using System;
using System.Threading.Tasks;

namespace Foundation.Pricing.Services
{
    public interface IPriceClientResolver
    {
        /// <summary>
        /// Builds the client half of a Grula price query.
        /// </summary>
        /// <param name="clientId">
        /// The client to price for. Implementations fall into two kinds and they are not
        /// interchangeable: a principal-based resolver takes the client from the authenticated
        /// user and rejects a non-null <paramref name="clientId"/> rather than pricing for
        /// somebody else, while a lookup-based resolver requires it. Pass <c>null</c> only when
        /// the caller means "whoever is signed in".
        /// </param>
        /// <param name="discountCode">
        /// Always supplied by the caller - it is request state resolved by
        /// <see cref="Baskets.DiscountCodeResolver"/>, never ambient.
        /// </param>
        /// <param name="token">
        /// Access token for resolvers that read the client from an API. Principal-based
        /// resolvers ignore it.
        /// </param>
        Task<PriceClient> ResolveAsync(Guid? clientId, string discountCode, string token);
    }
}
