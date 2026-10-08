using Foundation.Pricing.DiscountCodes;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    public class DiscountCodeValidatorTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        private static DiscountCodeValidator CreateValidator(IDiscountCodeLookup lookup)
        {
            return new DiscountCodeValidator(lookup, Substitute.For<ILogger<DiscountCodeValidator>>());
        }

        private static IDiscountCodeLookup LookupReturning(DiscountCodeValidation validation)
        {
            var lookup = Substitute.For<IDiscountCodeLookup>();

            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(validation));

            return lookup;
        }

        [Fact]
        public async Task ValidateAsync_WhenTheClientIsNullOrEmpty_ReturnsClientUnknownWithoutALookup()
        {
            var lookup = LookupReturning(DiscountCodeValidation.Valid("SUMMER25"));
            var validator = CreateValidator(lookup);

            var withoutClient = await validator.ValidateAsync(null, "SUMMER25", "token");
            var withEmptyClient = await validator.ValidateAsync(Guid.Empty, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.ClientUnknown, withoutClient.Status);
            Assert.Equal(DiscountCodeValidationStatus.ClientUnknown, withEmptyClient.Status);
            await lookup.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ValidateAsync_WhenTheCodeIsBlank_ReturnsNotFoundWithoutALookup(string code)
        {
            var lookup = LookupReturning(DiscountCodeValidation.Valid("SUMMER25"));

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, code, "token");

            Assert.Equal(DiscountCodeValidationStatus.NotFound, validation.Status);
            await lookup.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheCodeIsLongerThanTheLimit_IsNotApplicableWithoutALookup()
        {
            var lookup = LookupReturning(DiscountCodeValidation.Valid("SUMMER25"));

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, new string('A', DiscountCodeLimits.MaxLength + 1), "token");

            Assert.Equal(DiscountCodeValidationStatus.NotApplicable, validation.Status);
            await lookup.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheLookupAnswersValid_ReturnsTheCanonicalSpelling()
        {
            var lookup = LookupReturning(DiscountCodeValidation.Valid("SUMMER25"));

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, "  summer25 ", "token");

            Assert.True(validation.IsValid);
            Assert.Equal("SUMMER25", validation.DiscountCode);
            await lookup.Received(1).ValidateAsync(ClientId, "summer25", "token", Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task ValidateAsync_WhenTheLookupAnswersNotApplicable_NeverPassesACodeThrough()
        {
            var lookup = LookupReturning(new DiscountCodeValidation { Status = DiscountCodeValidationStatus.NotAssigned, DiscountCode = "SUMMER25" });

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.False(validation.IsValid);
            Assert.Equal(DiscountCodeValidationStatus.NotAssigned, validation.Status);
            Assert.Null(validation.DiscountCode);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheLookupFailsWithAnHttpError_FailsClosedToUnavailable()
        {
            var lookup = Substitute.For<IDiscountCodeLookup>();
            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("boom"));

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheLookupTimesOut_FailsClosedToUnavailable()
        {
            var lookup = Substitute.For<IDiscountCodeLookup>();

            // The HTTP client signals its own timeout with a cancellation that the caller did not ask for.
            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new TaskCanceledException("timeout"));

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, "SUMMER25", "token", CancellationToken.None);

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheCallerCancels_PropagatesTheCancellationInsteadOfAnAnswer()
        {
            using var cancellation = new CancellationTokenSource();
            var lookup = Substitute.For<IDiscountCodeLookup>();

            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(call =>
                {
                    cancellation.Cancel();
                    call.ArgAt<CancellationToken>(3).ThrowIfCancellationRequested();

                    return Task.FromResult(DiscountCodeValidation.Valid("SUMMER25"));
                });

            await Assert.ThrowsAnyAsync<OperationCanceledException>(
                () => CreateValidator(lookup).ValidateAsync(ClientId, "SUMMER25", "token", cancellation.Token));
        }

        [Fact]
        public async Task ValidateAsync_AfterACancelledAttempt_AsksAgainInsteadOfRemembering()
        {
            using var cancellation = new CancellationTokenSource();
            var calls = 0;
            var lookup = Substitute.For<IDiscountCodeLookup>();

            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(call =>
                {
                    if (Interlocked.Increment(ref calls) == 1)
                    {
                        cancellation.Cancel();
                        call.ArgAt<CancellationToken>(3).ThrowIfCancellationRequested();
                    }

                    return Task.FromResult(DiscountCodeValidation.Valid("SUMMER25"));
                });

            var validator = CreateValidator(lookup);

            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => validator.ValidateAsync(ClientId, "SUMMER25", "token", cancellation.Token));
            var validation = await validator.ValidateAsync(ClientId, "SUMMER25", "token", CancellationToken.None);

            Assert.True(validation.IsValid);
            Assert.Equal(2, calls);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheLookupReturnsNothing_FailsClosedToUnavailable()
        {
            var validation = await CreateValidator(LookupReturning(null)).ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task ValidateAsync_WhenValidComesWithoutACanonicalCode_FailsClosedToUnavailable(string canonical)
        {
            var validation = await CreateValidator(LookupReturning(DiscountCodeValidation.Valid(canonical))).ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
            Assert.Null(validation.DiscountCode);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheCanonicalCodeIsLongerThanTheLimit_FailsClosedToUnavailable()
        {
            var tooLong = new string('A', DiscountCodeLimits.MaxLength + 1);

            var validation = await CreateValidator(LookupReturning(DiscountCodeValidation.Valid(tooLong))).ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
        }

        [Fact]
        public async Task ValidateAsync_ForTheSameTrimmedCodeIgnoringCase_AsksOnce()
        {
            var lookup = LookupReturning(DiscountCodeValidation.Valid("SUMMER25"));
            var validator = CreateValidator(lookup);

            var first = await validator.ValidateAsync(ClientId, "summer25", "token");
            var second = await validator.ValidateAsync(ClientId, " Summer25 ", "token");

            Assert.Equal("SUMMER25", first.DiscountCode);
            Assert.Equal("SUMMER25", second.DiscountCode);
            await lookup.ReceivedWithAnyArgs(1).ValidateAsync(default, default, default, default);
        }

        [Fact]
        public async Task ValidateAsync_WhenAskedAgainWithTheCanonicalSpelling_ReusesTheEarlierAnswer()
        {
            // The basket seam asks with the user's spelling and the pricing seam then asks with the canonical one.
            var lookup = LookupReturning(DiscountCodeValidation.Valid("SUMMER25"));
            var validator = CreateValidator(lookup);

            var typedByTheUser = await validator.ValidateAsync(ClientId, "summer25", "token");
            var canonical = await validator.ValidateAsync(ClientId, typedByTheUser.DiscountCode, "token");

            Assert.True(canonical.IsValid);
            await lookup.ReceivedWithAnyArgs(1).ValidateAsync(default, default, default, default);
        }

        [Fact]
        public async Task ValidateAsync_ForConcurrentCalls_SharesOneLookup()
        {
            var release = new TaskCompletionSource<DiscountCodeValidation>();
            var lookup = Substitute.For<IDiscountCodeLookup>();

            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(release.Task);

            var validator = CreateValidator(lookup);
            var first = validator.ValidateAsync(ClientId, "SUMMER25", "token");
            var second = validator.ValidateAsync(ClientId, "summer25", "token");

            release.SetResult(DiscountCodeValidation.Valid("SUMMER25"));

            Assert.True((await first).IsValid);
            Assert.True((await second).IsValid);
            await lookup.ReceivedWithAnyArgs(1).ValidateAsync(default, default, default, default);
        }

        [Fact]
        public async Task ValidateAsync_ForADifferentTokenOrClient_AsksAgain()
        {
            var lookup = LookupReturning(DiscountCodeValidation.Valid("SUMMER25"));
            var validator = CreateValidator(lookup);

            await validator.ValidateAsync(ClientId, "SUMMER25", "token");
            await validator.ValidateAsync(ClientId, "SUMMER25", "another-token");
            await validator.ValidateAsync(Guid.NewGuid(), "SUMMER25", "token");

            await lookup.ReceivedWithAnyArgs(3).ValidateAsync(default, default, default, default);
        }

        [Fact]
        public async Task ValidateAsync_RemembersUnavailableForTheRestOfTheRequest_ButNotForTheNextOne()
        {
            var lookup = Substitute.For<IDiscountCodeLookup>();
            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("down"));

            var request = CreateValidator(lookup);

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, (await request.ValidateAsync(ClientId, "SUMMER25", "token")).Status);
            Assert.Equal(DiscountCodeValidationStatus.Unavailable, (await request.ValidateAsync(ClientId, "SUMMER25", "token")).Status);
            await lookup.ReceivedWithAnyArgs(1).ValidateAsync(default, default, default, default);

            // The next HTTP request gets a fresh validator and so retries.
            lookup.ClearReceivedCalls();
            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(DiscountCodeValidation.Valid("SUMMER25")));

            var nextRequest = CreateValidator(lookup);

            Assert.True((await nextRequest.ValidateAsync(ClientId, "SUMMER25", "token")).IsValid);
            await lookup.ReceivedWithAnyArgs(1).ValidateAsync(default, default, default, default);
        }
    }

    public class DiscountCodeValidationMappingTests
    {
        [Fact]
        public void FromApiResponse_ForValid_CarriesTheCanonicalCode()
        {
            var validation = DiscountCodeValidation.FromApiResponse("Valid", "SUMMER25");

            Assert.True(validation.IsValid);
            Assert.Equal("SUMMER25", validation.DiscountCode);
        }

        [Theory]
        [InlineData("NotFound", DiscountCodeValidationStatus.NotFound)]
        [InlineData("Disabled", DiscountCodeValidationStatus.Disabled)]
        [InlineData("NotAssigned", DiscountCodeValidationStatus.NotAssigned)]
        [InlineData("ClientUnknown", DiscountCodeValidationStatus.ClientUnknown)]
        [InlineData("NotApplicable", DiscountCodeValidationStatus.NotApplicable)]
        public void FromApiResponse_ForAnInapplicableStatus_MapsItWithoutACode(string status, DiscountCodeValidationStatus expected)
        {
            var validation = DiscountCodeValidation.FromApiResponse(status, "SUMMER25");

            Assert.Equal(expected, validation.Status);
            Assert.Null(validation.DiscountCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("Banana")]
        [InlineData("valid")]
        [InlineData("42")]
        [InlineData("Unavailable")]
        public void FromApiResponse_ForAMissingOrUnknownStatus_IsUnavailable(string status)
        {
            var validation = DiscountCodeValidation.FromApiResponse(status, "SUMMER25");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
            Assert.Null(validation.DiscountCode);
        }
    }
}
