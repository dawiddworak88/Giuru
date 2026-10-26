using Foundation.Pricing.Baskets;
using Foundation.Pricing.Services;
using NSubstitute;
using System;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Orders.Baskets
{
    public class BasketDiscountCodeCoordinatorTests
    {
        private const string Invalid = "invalid";
        private const string Removed = "removed";
        private const string Empty = "empty";

        private static Task<BasketDiscountCodeOutcome> ResolveAsync(
            IDiscountCodeEligibilityRepository eligibility,
            bool hasDiscountCode,
            string requested,
            string persisted,
            bool hasItems = true,
            bool isGrulaConfigured = true)
        {
            return BasketDiscountCodeCoordinator.ResolveAsync(
                isGrulaConfigured,
                hasDiscountCode,
                requested,
                hasItems,
                () => Task.FromResult(persisted),
                eligibility,
                "token",
                "en",
                Guid.NewGuid(),
                () => Empty,
                () => Invalid,
                () => Removed);
        }

        [Fact]
        public async Task ValidNewCode_IsAppliedWithItsStoredSpelling()
        {
            var outcome = await ResolveAsync(DiscountCodeEligibilityStubs.Knowing("SUMMER10"), true, "  summer10 ", null);

            Assert.False(outcome.IsRejected);
            Assert.Equal("SUMMER10", outcome.DiscountCode);
            Assert.Null(outcome.RemovedMessage);
        }

        [Fact]
        public async Task InvalidNewCode_IsRejectedWithTheGenericMessage()
        {
            var outcome = await ResolveAsync(DiscountCodeEligibilityStubs.Knowing("SUMMER10"), true, "WINTER", "SUMMER10");

            Assert.True(outcome.IsRejected);
            Assert.Equal(Invalid, outcome.RejectionMessage);
            Assert.Null(outcome.DiscountCode);
        }

        [Fact]
        public async Task StillValidStoredCode_IsKeptWhenTheRequestOmitsIt()
        {
            var outcome = await ResolveAsync(DiscountCodeEligibilityStubs.Knowing("SUMMER10"), false, null, "SUMMER10");

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
            var outcome = await ResolveAsync(DiscountCodeEligibilityStubs.Knowing(), hasDiscountCode, requested, "SUMMER10");

            Assert.False(outcome.IsRejected);
            Assert.Null(outcome.DiscountCode);
            Assert.Equal(Removed, outcome.RemovedMessage);
        }

        [Fact]
        public async Task StaleStoredCode_ResentByTheClientIsDroppedNotRejected()
        {
            var outcome = await ResolveAsync(DiscountCodeEligibilityStubs.Knowing(), true, "summer10", "SUMMER10");

            Assert.False(outcome.IsRejected);
            Assert.Null(outcome.DiscountCode);
            Assert.Equal(Removed, outcome.RemovedMessage);
        }

        [Fact]
        public async Task ExplicitRemoval_ClearsTheCodeWithoutAnyCheck()
        {
            var eligibility = DiscountCodeEligibilityStubs.Knowing();

            var outcome = await ResolveAsync(eligibility, true, null, "SUMMER10");

            Assert.False(outcome.IsRejected);
            Assert.Null(outcome.DiscountCode);
            Assert.Null(outcome.RemovedMessage);
            await eligibility.DidNotReceiveWithAnyArgs().GetValidDiscountCodeAsync(default, default, default, default);
        }

        [Fact]
        public async Task NoStoredCodeAndNoRequest_NeverCallsTheCheck()
        {
            var eligibility = DiscountCodeEligibilityStubs.Knowing();

            var outcome = await ResolveAsync(eligibility, false, null, null);

            Assert.Null(outcome.DiscountCode);
            Assert.Null(outcome.RemovedMessage);
            await eligibility.DidNotReceiveWithAnyArgs().GetValidDiscountCodeAsync(default, default, default, default);
        }

        [Fact]
        public async Task NewCodeOnAnEmptyBasket_IsRejectedBeforeAnyCheck()
        {
            var eligibility = DiscountCodeEligibilityStubs.Knowing("SUMMER10");

            var outcome = await ResolveAsync(eligibility, true, "SUMMER10", null, hasItems: false);

            Assert.True(outcome.IsRejected);
            Assert.Equal(Empty, outcome.RejectionMessage);
            await eligibility.DidNotReceiveWithAnyArgs().GetValidDiscountCodeAsync(default, default, default, default);
        }

        [Fact]
        public async Task WhenGrulaIsNotConfigured_IncomingChangesAreIgnoredAndTheStoredCodeIsKept()
        {
            var eligibility = DiscountCodeEligibilityStubs.Knowing();

            var outcome = await ResolveAsync(eligibility, true, "OTHER", "SUMMER10", isGrulaConfigured: false);

            Assert.Equal("SUMMER10", outcome.DiscountCode);
            Assert.Null(outcome.RemovedMessage);
            await eligibility.DidNotReceiveWithAnyArgs().GetValidDiscountCodeAsync(default, default, default, default);
        }

        [Fact]
        public async Task TheCheckIsMadeForTheGivenClient()
        {
            var eligibility = Substitute.For<IDiscountCodeEligibilityRepository>();
            var clientId = Guid.NewGuid();

            await BasketDiscountCodeCoordinator.ResolveAsync(
                true, true, "SUMMER10", true, () => Task.FromResult<string>(null), eligibility, "token", "en", clientId,
                () => Empty, () => Invalid, () => Removed);

            await eligibility.Received(1).GetValidDiscountCodeAsync("token", "en", clientId, "SUMMER10");
        }
    }
}
