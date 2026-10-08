using Foundation.Pricing.Baskets;
using Foundation.Pricing.DiscountCodes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    // The basket write seam of discount code enforcement: what a save does with a code, and what the user is told.
    public class BasketDiscountCodeCoordinatorTests
    {
        private const string AppliedToEmptyBasket = "applied-to-empty-basket";

        private sealed class Probe
        {
            public int PersistedReads { get; private set; }
            public List<string> Validated { get; } = new();
            public List<DiscountCodeValidationStatus> RejectionStatuses { get; } = new();

            public Func<Task<string>> ReadPersisted(string persisted)
            {
                return () =>
                {
                    PersistedReads++;

                    return Task.FromResult(persisted);
                };
            }

            public Func<string, Task<DiscountCodeValidation>> Validate(DiscountCodeValidation validation)
            {
                return code =>
                {
                    Validated.Add(code);

                    return Task.FromResult(validation);
                };
            }

            public Func<DiscountCodeValidationStatus, string> Reject()
            {
                return status =>
                {
                    RejectionStatuses.Add(status);

                    return $"rejected:{status}";
                };
            }
        }

        private static Task<BasketDiscountCodeOutcome> ResolveAsync(
            Probe probe,
            bool isGrulaConfigured = true,
            bool hasDiscountCode = true,
            string requested = "summer25",
            bool hasItems = true,
            string persisted = null,
            bool enforced = true,
            DiscountCodeValidation validation = null)
        {
            return BasketDiscountCodeCoordinator.ResolveAsync(
                isGrulaConfigured,
                hasDiscountCode,
                requested,
                hasItems,
                probe.ReadPersisted(persisted),
                () => AppliedToEmptyBasket,
                enforced,
                probe.Validate(validation ?? DiscountCodeValidation.Valid("SUMMER25")),
                probe.Reject());
        }

        [Fact]
        public async Task ResolveAsync_WhenGrulaIsNotConfigured_KeepsTheStoredCodeAndValidatesNothing()
        {
            var probe = new Probe();

            var outcome = await ResolveAsync(probe, isGrulaConfigured: false, requested: "other", persisted: "STORED");

            Assert.Equal("STORED", outcome.DiscountCode);
            Assert.False(outcome.IsRejected);
            Assert.False(outcome.IsRemoved);
            Assert.Empty(probe.Validated);
        }

        [Fact]
        public async Task ResolveAsync_WhenEnforcementIsOff_KeepsTheExistingSemanticsAndValidatesNothing()
        {
            var probe = new Probe();

            var outcome = await ResolveAsync(probe, requested: "  typed  ", enforced: false);

            Assert.Equal("typed", outcome.DiscountCode);
            Assert.Empty(probe.Validated);
        }

        [Fact]
        public async Task ResolveAsync_WhenEnforcementIsOff_StillRejectsACodeAppliedToAnEmptyBasket()
        {
            var probe = new Probe();

            var outcome = await ResolveAsync(probe, hasItems: false, enforced: false);

            Assert.Equal(AppliedToEmptyBasket, outcome.RejectionMessage);
        }

        [Fact]
        public async Task ResolveAsync_ForANewValidCode_ReturnsTheCanonicalSpelling()
        {
            var probe = new Probe();

            var outcome = await ResolveAsync(probe, requested: "summer25", validation: DiscountCodeValidation.Valid("SUMMER25"));

            Assert.Equal("SUMMER25", outcome.DiscountCode);
            Assert.False(outcome.IsRejected);
            Assert.Equal(new[] { "summer25" }, probe.Validated);
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.NotFound)]
        [InlineData(DiscountCodeValidationStatus.Disabled)]
        [InlineData(DiscountCodeValidationStatus.NotAssigned)]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown)]
        [InlineData(DiscountCodeValidationStatus.NotApplicable)]
        public async Task ResolveAsync_ForANewCodeThatIsNotApplicable_IsRejectedWithTheStatusAndTheStoredCodeIsUntouched(DiscountCodeValidationStatus status)
        {
            var probe = new Probe();

            var outcome = await ResolveAsync(probe, requested: "other", persisted: "STORED", validation: DiscountCodeValidation.Invalid(status));

            Assert.True(outcome.IsRejected);
            Assert.False(outcome.IsRemoved);
            Assert.Null(outcome.DiscountCode);
            Assert.Equal($"rejected:{status}", outcome.RejectionMessage);
            Assert.Equal(new[] { status }, probe.RejectionStatuses);
        }

        [Fact]
        public async Task ResolveAsync_WhenTheStoredCodeIsNoLongerApplicable_RemovesItAndReportsIt()
        {
            var probe = new Probe();

            // The save does not mention the code, so the stored one is what is validated.
            var outcome = await ResolveAsync(
                probe,
                hasDiscountCode: false,
                requested: null,
                persisted: "SUMMER25",
                validation: DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned));

            Assert.True(outcome.IsRemoved);
            Assert.False(outcome.IsRejected);
            Assert.Equal("SUMMER25", outcome.RemovedDiscountCode);
            Assert.Null(outcome.DiscountCode);
            Assert.Empty(probe.RejectionStatuses);
        }

        [Theory]
        [InlineData("summer25", "SUMMER25")]
        [InlineData("SUMMER25", "summer25")]
        [InlineData("  Summer25 ", "SUMMER25")]
        public async Task ResolveAsync_WhenTheExplicitCodeIsACaseVariantOfTheStoredOne_RemovesItInsteadOfRejecting(string requested, string persisted)
        {
            var probe = new Probe();

            var outcome = await ResolveAsync(
                probe,
                requested: requested,
                persisted: persisted,
                validation: DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Disabled));

            Assert.True(outcome.IsRemoved);
            Assert.Equal(persisted.Trim(), outcome.RemovedDiscountCode);
            Assert.Null(outcome.DiscountCode);
        }

        [Fact]
        public async Task ResolveAsync_WhenTheStoredCodeCannotBeVerified_RejectsTheSaveInsteadOfRemovingIt()
        {
            var probe = new Probe();

            var outcome = await ResolveAsync(
                probe,
                hasDiscountCode: false,
                requested: null,
                persisted: "SUMMER25",
                validation: DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable));

            Assert.True(outcome.IsRejected);
            Assert.False(outcome.IsRemoved);
            Assert.Equal($"rejected:{DiscountCodeValidationStatus.Unavailable}", outcome.RejectionMessage);
        }

        [Fact]
        public async Task ResolveAsync_WhenANewCodeCannotBeVerified_IsRejected()
        {
            var probe = new Probe();

            var outcome = await ResolveAsync(probe, requested: "other", persisted: "STORED", validation: DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable));

            Assert.True(outcome.IsRejected);
            Assert.Equal(new[] { DiscountCodeValidationStatus.Unavailable }, probe.RejectionStatuses);
        }

        [Fact]
        public async Task ResolveAsync_ForAnExplicitRemoval_ValidatesNothing()
        {
            var probe = new Probe();

            var outcome = await ResolveAsync(probe, requested: null, persisted: "SUMMER25");

            Assert.Null(outcome.DiscountCode);
            Assert.False(outcome.IsRejected);
            Assert.False(outcome.IsRemoved);
            Assert.Empty(probe.Validated);
        }

        [Fact]
        public async Task ResolveAsync_ForABasketWithoutACode_ValidatesNothing()
        {
            var probe = new Probe();

            var outcome = await ResolveAsync(probe, hasDiscountCode: false, requested: null, persisted: null);

            Assert.Null(outcome.DiscountCode);
            Assert.Empty(probe.Validated);
        }

        [Fact]
        public async Task ResolveAsync_ForACodeAppliedToAnEmptyBasket_IsRejectedBeforeAnyValidation()
        {
            var probe = new Probe();

            var outcome = await ResolveAsync(probe, hasItems: false, requested: "other", persisted: "STORED");

            Assert.Equal(AppliedToEmptyBasket, outcome.RejectionMessage);
            Assert.Empty(probe.Validated);
        }

        [Fact]
        public async Task ResolveAsync_WhenTheCodeNeedsAStoredReadForResolutionAndComparison_ReadsTheStoredCodeOnce()
        {
            var probe = new Probe();

            // An omitted code is resolved against the stored one (read 1); the invalid verdict compares against it again.
            await ResolveAsync(
                probe,
                hasDiscountCode: false,
                requested: null,
                persisted: "SUMMER25",
                validation: DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned));

            Assert.Equal(1, probe.PersistedReads);
        }

        [Fact]
        public async Task ResolveAsync_WhenAnExplicitCodeIsRejected_ReadsTheStoredCodeOnlyForTheComparison()
        {
            var probe = new Probe();

            await ResolveAsync(probe, requested: "other", persisted: "STORED", validation: DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotFound));

            Assert.Equal(1, probe.PersistedReads);
        }

        [Fact]
        public async Task ResolveAsync_WhenTheNewCodeIsValid_NeverReadsTheStoredCode()
        {
            var probe = new Probe();

            await ResolveAsync(probe, requested: "summer25", persisted: "STORED");

            Assert.Equal(0, probe.PersistedReads);
        }

        [Fact]
        public async Task ResolveAsync_WhenEnforcedWithoutAValidationFunction_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => BasketDiscountCodeCoordinator.ResolveAsync(
                true, true, "summer25", true, () => Task.FromResult<string>(null), () => AppliedToEmptyBasket, isDiscountCodeEnforced: true));
        }
    }
}
