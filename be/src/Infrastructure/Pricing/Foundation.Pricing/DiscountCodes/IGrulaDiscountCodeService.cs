using System.Threading;
using System.Threading.Tasks;

namespace Foundation.Pricing.DiscountCodes
{
    public enum GrulaDiscountCodeLookupStatus
    {
        /// <summary>The code is a driver item of the discount code driver in Grula.</summary>
        Exists,

        /// <summary>Grula answered and the code is not defined there.</summary>
        Missing,

        /// <summary>Grula could not give a usable answer: the code must not be saved.</summary>
        Unavailable,

        /// <summary>Grula is not configured: the code must not be saved.</summary>
        NotConfigured
    }

    public sealed class GrulaDiscountCodeLookup
    {
        public GrulaDiscountCodeLookupStatus Status { get; init; }

        /// <summary>Grula's spelling of the code. Set only when <see cref="Status"/> is <see cref="GrulaDiscountCodeLookupStatus.Exists"/>.</summary>
        public string DiscountCode { get; init; }

        public static GrulaDiscountCodeLookup Exists(string discountCode)
        {
            return new GrulaDiscountCodeLookup { Status = GrulaDiscountCodeLookupStatus.Exists, DiscountCode = discountCode };
        }

        public static GrulaDiscountCodeLookup Of(GrulaDiscountCodeLookupStatus status)
        {
            return new GrulaDiscountCodeLookup { Status = status };
        }
    }

    /// <summary>
    /// Answers "does this code exist in Grula". Only the web apps hold Grula credentials, so this is checked when
    /// a seller creates a code, which keeps Grula out of the per-request verification path.
    /// </summary>
    public interface IGrulaDiscountCodeService
    {
        Task<GrulaDiscountCodeLookup> FindAsync(string discountCode, CancellationToken cancellationToken = default);
    }
}
