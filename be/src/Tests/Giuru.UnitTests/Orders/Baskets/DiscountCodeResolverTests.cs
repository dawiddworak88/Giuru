using System.Collections.Generic;
using BuyerDiscountCodeResolver = Buyer.Web.Shared.Services.Baskets.DiscountCodeResolver;
using SellerDiscountCodeResolver = Seller.Web.Shared.Services.Baskets.DiscountCodeResolver;

namespace Giuru.UnitTests.Orders.Baskets
{
    /// <summary>
    /// The buyer and the seller each carry their own copy of the resolver, so both are
    /// held to the same matrix here to keep them from drifting apart.
    /// </summary>
    public abstract class DiscountCodeResolverTests
    {
        protected abstract (string DiscountCode, bool IsAppliedToEmptyBasket) Resolve(
            bool hasDiscountCode,
            string requestedDiscountCode,
            string persistedDiscountCode,
            bool hasItems);

        protected abstract string ResolveDiscountCode(bool hasDiscountCode, string requestedDiscountCode, string persistedDiscountCode);

        protected abstract bool RequiresPersistedDiscountCode(bool hasDiscountCode, string requestedDiscountCode, bool hasItems);

        public static TheoryData<bool, string, string, bool, string, bool> ResolutionCases => new()
        {
            // hasDiscountCode, requested, persisted, hasItems | expected code, applied to empty basket

            // The request says nothing about the code, so the stored one stays authoritative.
            { false, null, "SUMMER25", true, "SUMMER25", false },
            { false, null, null, true, null, false },
            { false, null, "   ", true, null, false },
            { false, null, "  SUMMER25  ", true, "SUMMER25", false },

            // An explicit null removes the persisted code.
            { true, null, "SUMMER25", true, null, false },
            { true, null, null, true, null, false },
            { true, null, "SUMMER25", false, null, false },

            // A blank code means "unchanged", not "remove" - it must not swallow the stored code.
            { true, "   ", "SUMMER25", true, "SUMMER25", false },
            { true, "", "SUMMER25", true, "SUMMER25", false },
            { true, "   ", null, true, null, false },

            // An explicit code wins outright and is trimmed before it is priced with and stored.
            { true, "SUMMER25", null, true, "SUMMER25", false },
            { true, "  SUMMER25  ", "WINTER25", true, "SUMMER25", false },

            // Applying a *new* code to a basket with no lines is rejected by the callers.
            { true, "SUMMER25", null, false, "SUMMER25", true },
            { true, "SUMMER25", "WINTER25", false, "SUMMER25", true },

            // Re-sending the code already on the basket is how the client empties a
            // discounted basket, so it must not be flagged.
            { true, "SUMMER25", "SUMMER25", false, "SUMMER25", false },
            { true, "  SUMMER25  ", "SUMMER25", false, "SUMMER25", false }
        };

        /// <summary>The same requests as <see cref="ResolutionCases"/>, without the expectations.</summary>
        public static TheoryData<bool, string, string, bool> RequestCases
        {
            get
            {
                var requests = new TheoryData<bool, string, string, bool>();

                foreach (var resolutionCase in ResolutionCases)
                {
                    requests.Add(
                        (bool)resolutionCase[0],
                        (string)resolutionCase[1],
                        (string)resolutionCase[2],
                        (bool)resolutionCase[3]);
                }

                return requests;
            }
        }

        [Theory]
        [MemberData(nameof(ResolutionCases))]
        public void Resolve_ReconcilesTheRequestedCodeWithThePersistedOne(
            bool hasDiscountCode,
            string requestedDiscountCode,
            string persistedDiscountCode,
            bool hasItems,
            string expectedDiscountCode,
            bool expectedIsAppliedToEmptyBasket)
        {
            var resolution = Resolve(hasDiscountCode, requestedDiscountCode, persistedDiscountCode, hasItems);

            Assert.Equal(expectedDiscountCode, resolution.DiscountCode);
            Assert.Equal(expectedIsAppliedToEmptyBasket, resolution.IsAppliedToEmptyBasket);
        }

        [Theory]
        [MemberData(nameof(RequestCases))]
        public void ResolveDiscountCode_AgreesWithResolve(
            bool hasDiscountCode,
            string requestedDiscountCode,
            string persistedDiscountCode,
            bool hasItems)
        {
            // The multipart upload path resolves the code without the full resolution, so
            // both entry points have to produce the same code for the same request.
            var discountCode = ResolveDiscountCode(hasDiscountCode, requestedDiscountCode, persistedDiscountCode);

            Assert.Equal(Resolve(hasDiscountCode, requestedDiscountCode, persistedDiscountCode, hasItems).DiscountCode, discountCode);
        }

        [Theory]
        // An omitted code can only be resolved against what is stored.
        [InlineData(false, null, true, true)]
        [InlineData(false, null, false, true)]
        // A blank code means "unchanged", so the stored one is needed too.
        [InlineData(true, "   ", true, true)]
        [InlineData(true, "", false, true)]
        // An explicit removal is self-contained.
        [InlineData(true, null, true, false)]
        [InlineData(true, null, false, false)]
        // An explicit code is self-contained as long as there are lines. Without lines the
        // stored code still decides whether this is a rejected apply or an allowed clear.
        [InlineData(true, "SUMMER25", true, false)]
        [InlineData(true, "SUMMER25", false, true)]
        public void RequiresPersistedDiscountCode_OnlyAsksForTheBasketWhenTheRequestCannotResolveOnItsOwn(
            bool hasDiscountCode,
            string requestedDiscountCode,
            bool hasItems,
            bool expected)
        {
            Assert.Equal(expected, RequiresPersistedDiscountCode(hasDiscountCode, requestedDiscountCode, hasItems));
        }

        [Theory]
        [MemberData(nameof(RequestCases))]
        public void Resolve_WhenThePersistedCodeIsNotRequired_IgnoresIt(
            bool hasDiscountCode,
            string requestedDiscountCode,
            string persistedDiscountCode,
            bool hasItems)
        {
            if (RequiresPersistedDiscountCode(hasDiscountCode, requestedDiscountCode, hasItems))
            {
                return;
            }

            // Callers skip the basket read and pass null in this case, so the outcome must
            // not depend on what is actually stored - otherwise the skip silently drops a code.
            var withoutPersisted = Resolve(hasDiscountCode, requestedDiscountCode, null, hasItems);

            foreach (var persisted in new List<string> { null, "", "   ", "SUMMER25", "WINTER25", persistedDiscountCode })
            {
                var withPersisted = Resolve(hasDiscountCode, requestedDiscountCode, persisted, hasItems);

                Assert.Equal(withoutPersisted.DiscountCode, withPersisted.DiscountCode);
                Assert.Equal(withoutPersisted.IsAppliedToEmptyBasket, withPersisted.IsAppliedToEmptyBasket);
            }
        }
    }

    public class BuyerDiscountCodeResolverTests : DiscountCodeResolverTests
    {
        protected override (string DiscountCode, bool IsAppliedToEmptyBasket) Resolve(
            bool hasDiscountCode,
            string requestedDiscountCode,
            string persistedDiscountCode,
            bool hasItems)
        {
            var resolution = BuyerDiscountCodeResolver.Resolve(hasDiscountCode, requestedDiscountCode, persistedDiscountCode, hasItems);

            return (resolution.DiscountCode, resolution.IsAppliedToEmptyBasket);
        }

        protected override string ResolveDiscountCode(bool hasDiscountCode, string requestedDiscountCode, string persistedDiscountCode)
        {
            return BuyerDiscountCodeResolver.ResolveDiscountCode(hasDiscountCode, requestedDiscountCode, persistedDiscountCode);
        }

        protected override bool RequiresPersistedDiscountCode(bool hasDiscountCode, string requestedDiscountCode, bool hasItems)
        {
            return BuyerDiscountCodeResolver.RequiresPersistedDiscountCode(hasDiscountCode, requestedDiscountCode, hasItems);
        }
    }

    public class SellerDiscountCodeResolverTests : DiscountCodeResolverTests
    {
        protected override (string DiscountCode, bool IsAppliedToEmptyBasket) Resolve(
            bool hasDiscountCode,
            string requestedDiscountCode,
            string persistedDiscountCode,
            bool hasItems)
        {
            var resolution = SellerDiscountCodeResolver.Resolve(hasDiscountCode, requestedDiscountCode, persistedDiscountCode, hasItems);

            return (resolution.DiscountCode, resolution.IsAppliedToEmptyBasket);
        }

        protected override string ResolveDiscountCode(bool hasDiscountCode, string requestedDiscountCode, string persistedDiscountCode)
        {
            return SellerDiscountCodeResolver.ResolveDiscountCode(hasDiscountCode, requestedDiscountCode, persistedDiscountCode);
        }

        protected override bool RequiresPersistedDiscountCode(bool hasDiscountCode, string requestedDiscountCode, bool hasItems)
        {
            return SellerDiscountCodeResolver.RequiresPersistedDiscountCode(hasDiscountCode, requestedDiscountCode, hasItems);
        }
    }
}
