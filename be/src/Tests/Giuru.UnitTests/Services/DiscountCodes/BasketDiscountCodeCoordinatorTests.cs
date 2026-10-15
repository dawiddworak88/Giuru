using Foundation.Pricing.Baskets;
using Foundation.Pricing.DiscountCodes;
using Microsoft.Extensions.Logging;
using NSubstitute;
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
            public ILogger Logger { get; } = Substitute.For<ILogger>();

            public async Task<BasketDiscountCodeOutcome> RunAsync(
                bool hasDiscountCode,
                string requestedDiscountCode,
                bool hasItems,
                string persistedDiscountCode,
                DiscountCodeValidation validation = null,
                bool isGrulaConfigured = true)
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
                    Logger);
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
        public async Task ResolveAsync_WhenGrulaIsNotConfigured_StillHonoursAnExplicitRemoval()
        {
            // Removing a code needs no pricing, so a basket is never stuck with a code while Grula is off.
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, null, true, "STORED25", isGrulaConfigured: false);

            Assert.False(outcome.IsRejected);
            Assert.False(outcome.IsDiscountCodeRemoved);
            Assert.Null(outcome.DiscountCode);
            Assert.Empty(scenario.ValidatedCodes);
            Assert.Equal(0, scenario.PersistedReads);
        }

        [Fact]
        public async Task ResolveAsync_ForAnExplicitRemoval_ContinuesWithoutACodeAndDoesNotValidate()
        {
            // A revoked stored code can always be removed.
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, null, true, "STORED25", DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Disabled));

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
            Assert.False(outcome.IsDiscountCodeRemoved);
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
            var scenario = new Scenario();

            await scenario.RunAsync(hasDiscountCode, requested, hasItems, "STORED25");

            Assert.True(scenario.PersistedReads <= 1);
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(true, "")]
        [InlineData(true, "   ")]
        [InlineData(true, "STORED25")]
        [InlineData(true, "  stored25 ")]
        public async Task ResolveAsync_WhenTheStoredCodeIsKept_ValidatesItsStoredSpellingForTheSaveAndUsesTheCanonicalSpelling(bool hasDiscountCode, string requested)
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(hasDiscountCode, requested, true, "STORED25", DiscountCodeValidation.Valid("Stored25"));

            Assert.False(outcome.IsRejected);
            Assert.Equal("Stored25", outcome.DiscountCode);
            Assert.Equal(new[] { "STORED25" }, scenario.ValidatedCodes);
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.NotAssigned)]
        [InlineData(DiscountCodeValidationStatus.Disabled)]
        [InlineData(DiscountCodeValidationStatus.NotFound)]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown)]
        [InlineData(DiscountCodeValidationStatus.NotApplicable)]
        public async Task ResolveAsync_RemovesAStoredCodeThatIsNoLongerApplicableAndContinuesWithoutIt(DiscountCodeValidationStatus status)
        {
            // A code stored while the basket was for another client, or revoked since, must not price this save - but it
            // must not block the save either: the basket is priced without it and the caller is told which code went.
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(false, null, true, " STORED25 ", DiscountCodeValidation.Invalid(status));

            Assert.False(outcome.IsRejected);
            Assert.True(outcome.IsDiscountCodeRemoved);
            Assert.Equal("STORED25", outcome.RemovedDiscountCode);
            Assert.Null(outcome.DiscountCode);
            Assert.Empty(scenario.RejectionStatuses);
        }

        [Theory]
        [InlineData("STORED25")]
        [InlineData(" stored25 ")]
        public async Task ResolveAsync_WhenARevokedStoredCodeIsSentAgain_RemovesIt(string requested)
        {
            // Sending the stored code again does not exempt it from validation.
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, requested, true, "STORED25", DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Disabled));

            Assert.False(outcome.IsRejected);
            Assert.Equal("STORED25", outcome.RemovedDiscountCode);
            Assert.Null(outcome.DiscountCode);
            Assert.Equal(new[] { "STORED25" }, scenario.ValidatedCodes);
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(true, "")]
        [InlineData(true, "stored25")]
        public async Task ResolveAsync_WhenTheStoredCodeCannotBeVerified_KeepsItAndLogsAWarning(bool hasDiscountCode, string requested)
        {
            // An outage of the lookup neither blocks the save nor changes the basket's prices.
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(hasDiscountCode, requested, true, "STORED25", DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable));

            Assert.False(outcome.IsRejected);
            Assert.False(outcome.IsDiscountCodeRemoved);
            Assert.Equal("STORED25", outcome.DiscountCode);
            Assert.Equal(new[] { "STORED25" }, scenario.ValidatedCodes);
            scenario.Logger.Received(1).Log(LogLevel.Warning, Arg.Any<EventId>(), Arg.Any<object>(), Arg.Any<Exception>(), Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public async Task ResolveAsync_WhenAReplacementCannotBeVerified_RejectsItAndKeepsTheStoredCode()
        {
            // Only the stored code is kept through an outage; a new one is never stored unchecked.
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, "NEW25", true, "STORED25", DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable));

            Assert.True(outcome.IsRejected);
            Assert.False(outcome.IsDiscountCodeRemoved);
            Assert.Equal($"rejected-{DiscountCodeValidationStatus.Unavailable}", outcome.RejectionMessage);
        }

        [Theory]
        [InlineData(false, null)]
        [InlineData(true, "stored25")]
        public async Task ResolveAsync_KeepsTheStoredCodeOfAnEmptiedBasketWithoutValidating(bool hasDiscountCode, string requested)
        {
            // Nothing is priced, and the code is validated again as soon as lines are added.
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(hasDiscountCode, requested, false, "STORED25", DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.NotAssigned));

            Assert.False(outcome.IsRejected);
            Assert.Equal("STORED25", outcome.DiscountCode);
            Assert.Empty(scenario.ValidatedCodes);
        }

        [Fact]
        public async Task ResolveAsync_WhenGrulaIsNotConfigured_KeepsAStoredCodeThatIsNotSentWithoutValidating()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(false, null, true, "STORED25", isGrulaConfigured: false);

            Assert.False(outcome.IsRejected);
            Assert.Equal("STORED25", outcome.DiscountCode);
            Assert.Empty(scenario.ValidatedCodes);
        }

        [Fact]
        public async Task ResolveAsync_ForAReplacement_ValidatesOnlyTheReplacement()
        {
            var scenario = new Scenario();

            var outcome = await scenario.RunAsync(true, "NEW25", true, "STORED25");

            Assert.Equal("NEW25", outcome.DiscountCode);
            Assert.Equal(new[] { "NEW25" }, scenario.ValidatedCodes);
        }
    }
}
