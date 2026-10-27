using Foundation.Extensions.Exceptions;
using Foundation.Pricing.Baskets;
using Microsoft.Extensions.Localization;
using NSubstitute;
using System;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Orders.Baskets
{
    public class BasketDiscountCodeCoordinatorTests
    {
        private const string Invalid = "DiscountCodeInvalid";
        private const string Removed = "DiscountCodeRemoved";
        private const string Empty = "DiscountCodeRequiresBasketItems";

        private static Task<BasketDiscountCodeOutcome> ResolveAsync(
            Func<string, Task<string>> getValidDiscountCode,
            bool hasDiscountCode,
            string requested,
            string persisted,
            bool hasItems = true,
            bool isGrulaConfigured = true)
        {
            var localizer = Substitute.For<IStringLocalizer>();

            localizer[Arg.Any<string>()].Returns(call => new LocalizedString(call.ArgAt<string>(0), call.ArgAt<string>(0)));

            return BasketDiscountCodeCoordinator.ResolveAsync(
                isGrulaConfigured,
                hasDiscountCode,
                requested,
                hasItems,
                () => Task.FromResult(persisted),
                getValidDiscountCode,
                localizer);
        }

        private static Func<string, Task<string>> Knowing(params string[] storedSpellings)
        {
            var eligibility = DiscountCodeEligibilityStubs.Knowing(storedSpellings);

            return code => eligibility.GetValidDiscountCodeAsync("token", "en", Guid.NewGuid(), code);
        }

        [Fact]
        public async Task ValidNewCode_IsAppliedWithItsStoredSpelling()
        {
            var outcome = await ResolveAsync(Knowing("SUMMER10"), true, "  summer10 ", null);

            Assert.False(outcome.IsRejected);
            Assert.Equal("SUMMER10", outcome.DiscountCode);
            Assert.Null(outcome.RemovedMessage);
        }

        [Fact]
        public async Task InvalidNewCode_IsRejectedWithTheGenericMessage()
        {
            var outcome = await ResolveAsync(Knowing("SUMMER10"), true, "WINTER", "SUMMER10");

            Assert.True(outcome.IsRejected);
            Assert.Equal(Invalid, outcome.RejectionMessage);
            Assert.Null(outcome.DiscountCode);
        }

        [Fact]
        public async Task StillValidStoredCode_IsKeptWhenTheRequestOmitsIt()
        {
            var outcome = await ResolveAsync(Knowing("SUMMER10"), false, null, "SUMMER10");

            Assert.False(outcome.IsRejected);
            Assert.Equal("SUMMER10", outcome.DiscountCode);
            Assert.Null(outcome.RemovedMessage);
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(true, "")]
        [InlineData(true, "   ")]
        public async Task StaleStoredCode_IsDroppedWithAMessageWhenTheRequestLeavesItUnchanged(bool hasDiscountCode, string requested)
        {
            var outcome = await ResolveAsync(Knowing(), hasDiscountCode, requested, "SUMMER10");

            Assert.False(outcome.IsRejected);
            Assert.Null(outcome.DiscountCode);
            Assert.Equal(Removed, outcome.RemovedMessage);
        }

        [Fact]
        public async Task StaleStoredCode_ResentByTheClientIsDroppedNotRejected()
        {
            var outcome = await ResolveAsync(Knowing(), true, "summer10", "SUMMER10");

            Assert.False(outcome.IsRejected);
            Assert.Null(outcome.DiscountCode);
            Assert.Equal(Removed, outcome.RemovedMessage);
        }

        // The cases below need no eligibility check, so they still succeed while Client.Api is unreachable.

        [Fact]
        public async Task ExplicitRemoval_ClearsTheCodeWithoutAnyCheck()
        {
            var outcome = await ResolveAsync(DiscountCodeEligibilityStubs.Unreachable(), true, null, "SUMMER10");

            Assert.False(outcome.IsRejected);
            Assert.Null(outcome.DiscountCode);
            Assert.Null(outcome.RemovedMessage);
        }

        [Fact]
        public async Task NoStoredCodeAndNoRequest_NeedsNoCheck()
        {
            var outcome = await ResolveAsync(DiscountCodeEligibilityStubs.Unreachable(), false, null, null);

            Assert.False(outcome.IsRejected);
            Assert.Null(outcome.DiscountCode);
            Assert.Null(outcome.RemovedMessage);
        }

        [Fact]
        public async Task NewCodeOnAnEmptyBasket_IsRejectedBeforeAnyCheck()
        {
            var outcome = await ResolveAsync(DiscountCodeEligibilityStubs.Unreachable(), true, "SUMMER10", null, hasItems: false);

            Assert.True(outcome.IsRejected);
            Assert.Equal(Empty, outcome.RejectionMessage);
        }

        [Fact]
        public async Task WhenGrulaIsNotConfigured_IncomingChangesAreIgnoredAndTheStoredCodeIsKept()
        {
            var outcome = await ResolveAsync(DiscountCodeEligibilityStubs.Unreachable(), true, "OTHER", "SUMMER10", isGrulaConfigured: false);

            Assert.Equal("SUMMER10", outcome.DiscountCode);
            Assert.Null(outcome.RemovedMessage);
        }

        [Fact]
        public async Task WhenTheCheckCannotBeMade_TheSaveFailsInsteadOfApplyingOrDroppingTheCode()
        {
            await Assert.ThrowsAsync<CustomException>(() => ResolveAsync(DiscountCodeEligibilityStubs.Unreachable(), false, null, "SUMMER10"));
        }
    }
}
