using Foundation.Pricing.Baskets;
using Foundation.Pricing.DiscountCodes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    public class BasketDiscountCodeCoordinatorTests
    {
        private const string EmptyBasketMessage = "empty-basket";

        /// <summary>Runs the coordinator with fakes that record what it asked for.</summary>
        private sealed class Scenario
        {
            public int PersistedReads { get; private set; }
            public List<string> ValidatedCodes { get; } = new();
            public List<DiscountCodeValidationStatus> RejectionStatuses { get; } = new();

            public async Task<BasketDiscountCodeOutcome> RunAsync(
                bool hasDiscountCode,
                string requestedDiscountCode,
                bool hasItems,
                string persistedDiscountCode,
                DiscountCodeValidation validation = null,
                bool isGrulaConfigured = true,
                bool revalidateStoredDiscountCode = false)
            {
                return await BasketDiscountCodeCoordinator.ResolveAsync(
                    isGrulaConfigured,
                    hasDiscountCode,
                    requestedDiscountCode,
                    hasItems,
                    () =>
                    {
                        PersistedReads++;

                        return Task.FromResult(persistedDiscountCode);
                    },
                    () => EmptyBasketMessage,
                    code =>
                    {
                        ValidatedCodes.Add(code);

                        return Task.FromResult(validation ?? DiscountCodeValidation.Valid(code));
                    },
                    status =>
                    {
                        RejectionStatuses.Add(status);

                        return $"rejected-{status}";
                    },
                    revalidateStoredDiscountCode);
            }
        }

        [Fact]
        public async Task ResolveAsync_WhenGrulaIsNotConfigured_KeepsTheStoredCodeIgnoresTheRequestAndDoesNotValidate()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, "NEW25", true, "STORED25", isGrulaConfigured: false);

            Assert.False(outcome.IsRejected);
            Assert.Equal("STORED25", outcome.DiscountCode);
            Assert.Empty(scenario.ValidatedCodes);
            Assert.Equal(1, scenario.PersistedReads);
        }

        [Fact]
        public async Task ResolveAsync_ForAnExplicitRemoval_ContinuesWithoutACodeAndDoesNotValidate()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, null, true, "STORED25");

            Assert.False(outcome.IsRejected);
            Assert.Null(outcome.DiscountCode);
            Assert.Empty(scenario.ValidatedCodes);
        }

        [Fact]
        public async Task ResolveAsync_ForARequestWithoutACodeOnABasketWithoutOne_ContinuesWithoutACode()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(false, null, true, null);

            Assert.False(outcome.IsRejected);
            Assert.Null(outcome.DiscountCode);
            Assert.Empty(scenario.ValidatedCodes);
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(true, "")]
        [InlineData(true, "   ")]
        public async Task ResolveAsync_WhenTheCodeIsOmittedOrBlank_KeepsTheStoredCodeWithoutValidating(bool hasDiscountCode, string requested)
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(hasDiscountCode, requested, true, "STORED25");

            Assert.False(outcome.IsRejected);
            Assert.Equal("STORED25", outcome.DiscountCode);
            Assert.Empty(scenario.ValidatedCodes);
        }

        [Theory]
        [InlineData("STORED25")]
        [InlineData("stored25")]
        [InlineData("  Stored25  ")]
        public async Task ResolveAsync_WhenTheRequestCarriesTheStoredCodeAgain_KeepsTheStoredSpellingWithoutValidating(string requested)
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, requested, true, "STORED25");

            Assert.False(outcome.IsRejected);
            Assert.Equal("STORED25", outcome.DiscountCode);
            Assert.Empty(scenario.ValidatedCodes);
        }

        [Fact]
        public async Task ResolveAsync_WhenTheStoredCodeIsSentAgainEvenThoughItWouldNoLongerBeApplicable_StillKeepsIt()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(
                true, "STORED25", true, "STORED25",
                validation: DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Disabled));

            Assert.False(outcome.IsRejected);
            Assert.Equal("STORED25", outcome.DiscountCode);
            Assert.Empty(scenario.ValidatedCodes);
        }

        [Fact]
        public async Task ResolveAsync_WhenAStoredCodeIsSentAgainInOtherCasingToEmptyTheBasket_IsNotTreatedAsApplyingACodeToAnEmptyBasket()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, "stored25", false, "STORED25");

            Assert.False(outcome.IsRejected);
            Assert.Equal("STORED25", outcome.DiscountCode);
            Assert.Empty(scenario.ValidatedCodes);
        }

        [Fact]
        public async Task ResolveAsync_WhenANewCodeIsAppliedToAnEmptyBasket_RejectsWithTheEmptyBasketMessageBeforeValidating()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, "NEW25", false, null);

            Assert.True(outcome.IsRejected);
            Assert.Equal(EmptyBasketMessage, outcome.RejectionMessage);
            Assert.Empty(scenario.ValidatedCodes);
            Assert.Empty(scenario.RejectionStatuses);
        }

        [Fact]
        public async Task ResolveAsync_WhenAReplacementIsAppliedToAnEmptyBasket_RejectsWithTheEmptyBasketMessageBeforeValidating()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, "NEW25", false, "STORED25");

            Assert.True(outcome.IsRejected);
            Assert.Equal(EmptyBasketMessage, outcome.RejectionMessage);
            Assert.Empty(scenario.ValidatedCodes);
        }

        [Fact]
        public async Task ResolveAsync_WhenANewCodeIsApplicable_ContinuesWithTheCanonicalSpelling()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, "  new25 ", true, null, DiscountCodeValidation.Valid("NEW25"));

            Assert.False(outcome.IsRejected);
            Assert.Equal("NEW25", outcome.DiscountCode);
            Assert.Equal(new[] { "new25" }, scenario.ValidatedCodes);
        }

        [Fact]
        public async Task ResolveAsync_WhenAReplacementIsApplicable_ContinuesWithTheCanonicalSpelling()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, "NEW25", true, "STORED25", DiscountCodeValidation.Valid("New25"));

            Assert.False(outcome.IsRejected);
            Assert.Equal("New25", outcome.DiscountCode);
            Assert.Equal(new[] { "NEW25" }, scenario.ValidatedCodes);
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.NotFound)]
        [InlineData(DiscountCodeValidationStatus.Disabled)]
        [InlineData(DiscountCodeValidationStatus.NotAssigned)]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown)]
        [InlineData(DiscountCodeValidationStatus.NotApplicable)]
        public async Task ResolveAsync_WhenANewCodeIsNotApplicable_RejectsWithTheMessageForThatStatus(DiscountCodeValidationStatus status)
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, "NEW25", true, null, DiscountCodeValidation.Invalid(status));

            Assert.True(outcome.IsRejected);
            Assert.Equal($"rejected-{status}", outcome.RejectionMessage);
            Assert.Null(outcome.DiscountCode);
        }

        [Fact]
        public async Task ResolveAsync_WhenAReplacementIsNotApplicable_RejectsInsteadOfDroppingTheStoredCode()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, "NEW25", true, "STORED25", DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned));

            Assert.True(outcome.IsRejected);
            Assert.Null(outcome.DiscountCode);
            Assert.Equal(new[] { DiscountCodeValidationStatus.NotAssigned }, scenario.RejectionStatuses);
        }

        [Fact]
        public async Task ResolveAsync_WhenANewCodeCannotBeVerified_RejectsSoThatItIsNeverStoredUnchecked()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, "NEW25", true, null, DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable));

            Assert.True(outcome.IsRejected);
            Assert.Equal($"rejected-{DiscountCodeValidationStatus.Unavailable}", outcome.RejectionMessage);
        }

        [Theory]
        [InlineData(true, "STORED25", true)]
        [InlineData(true, "NEW25", true)]
        [InlineData(true, "NEW25", false)]
        [InlineData(false, null, true)]
        [InlineData(true, null, true)]
        [InlineData(true, "  ", true)]
        public async Task ResolveAsync_ReadsTheStoredCodeAtMostOnce(bool hasDiscountCode, string requested, bool hasItems)
        {
            foreach (var revalidateStoredDiscountCode in new[] { false, true })
            {
                var scenario = new Scenario();

                await scenario.RunAsync(hasDiscountCode, requested, hasItems, "STORED25", revalidateStoredDiscountCode: revalidateStoredDiscountCode);

                Assert.True(scenario.PersistedReads <= 1);
            }
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(true, "")]
        [InlineData(true, "   ")]
        [InlineData(true, "STORED25")]
        [InlineData(true, "  stored25 ")]
        public async Task ResolveAsync_WhenStoredCodesAreRevalidated_ValidatesTheStoredCodeForTheSaveAndUsesTheCanonicalSpelling(bool hasDiscountCode, string requested)
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(hasDiscountCode, requested, true, "STORED25", DiscountCodeValidation.Valid("Stored25"), revalidateStoredDiscountCode: true);

            Assert.False(outcome.IsRejected);
            Assert.Equal("Stored25", outcome.DiscountCode);
            Assert.Equal(new[] { "STORED25" }, scenario.ValidatedCodes);
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.NotAssigned)]
        [InlineData(DiscountCodeValidationStatus.Disabled)]
        [InlineData(DiscountCodeValidationStatus.NotFound)]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown)]
        [InlineData(DiscountCodeValidationStatus.Unavailable)]
        public async Task ResolveAsync_WhenStoredCodesAreRevalidated_RejectsAStoredCodeThatIsNotApplicableForTheSave(DiscountCodeValidationStatus status)
        {
            // A code stored while the basket was for another client, or revoked since, must not price this save.
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(false, null, true, "STORED25", DiscountCodeValidation.Invalid(status), revalidateStoredDiscountCode: true);

            Assert.True(outcome.IsRejected);
            Assert.Null(outcome.DiscountCode);
            Assert.Equal(new[] { status }, scenario.RejectionStatuses);
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(true, "stored25")]
        public async Task ResolveAsync_WhenStoredCodesAreRevalidated_KeepsTheStoredCodeOfAnEmptiedBasketWithoutValidating(bool hasDiscountCode, string requested)
        {
            // Nothing is priced, and the code is validated again as soon as lines are added.
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(hasDiscountCode, requested, false, "STORED25", DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned), revalidateStoredDiscountCode: true);

            Assert.False(outcome.IsRejected);
            Assert.Equal("STORED25", outcome.DiscountCode);
            Assert.Empty(scenario.ValidatedCodes);
        }

        [Fact]
        public async Task ResolveAsync_WhenStoredCodesAreRevalidated_StillRemovesACodeWithoutValidating()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, null, true, "STORED25", DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned), revalidateStoredDiscountCode: true);

            Assert.False(outcome.IsRejected);
            Assert.Null(outcome.DiscountCode);
            Assert.Empty(scenario.ValidatedCodes);
        }

        [Fact]
        public async Task ResolveAsync_WhenStoredCodesAreRevalidated_ButGrulaIsNotConfigured_KeepsTheStoredCodeWithoutValidating()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(false, null, true, "STORED25", isGrulaConfigured: false, revalidateStoredDiscountCode: true);

            Assert.False(outcome.IsRejected);
            Assert.Equal("STORED25", outcome.DiscountCode);
            Assert.Empty(scenario.ValidatedCodes);
        }

        [Fact]
        public async Task ResolveAsync_WhenStoredCodesAreRevalidated_ValidatesAReplacementOnlyOnce()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, "NEW25", true, "STORED25", revalidateStoredDiscountCode: true);

            Assert.Equal("NEW25", outcome.DiscountCode);
            Assert.Equal(new[] { "NEW25" }, scenario.ValidatedCodes);
        }
    }
}
