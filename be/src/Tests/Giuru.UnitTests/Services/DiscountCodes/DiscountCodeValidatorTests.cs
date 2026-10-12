using Foundation.Pricing.DiscountCodes;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Net.Http;
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

            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>())
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
            await lookup.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
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
            await lookup.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheCodeIsLongerThanTheLimit_ReturnsNotFoundWithoutALookup()
        {
            var lookup = LookupReturning(DiscountCodeValidation.Valid("SUMMER25"));

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, new string('A', DiscountCodeLimits.MaxLength + 1), "token");

            Assert.Equal(DiscountCodeValidationStatus.NotFound, validation.Status);
            await lookup.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheLookupAnswersValid_ReturnsTheCanonicalSpelling()
        {
            var lookup = LookupReturning(DiscountCodeValidation.Valid("SUMMER25"));

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, "  summer25 ", "token");

            Assert.True(validation.IsValid);
            Assert.Equal("SUMMER25", validation.DiscountCode);
            await lookup.Received(1).ValidateAsync(ClientId, "summer25", "token");
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
            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>())
                .ThrowsAsync(new HttpRequestException("boom"));

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheLookupTimesOut_FailsClosedToUnavailable()
        {
            var lookup = Substitute.For<IDiscountCodeLookup>();

            // The HTTP client signals its own timeout with a cancellation that the caller did not ask for.
            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>())
                .ThrowsAsync(new TaskCanceledException("timeout"));

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheLookupThrowsSynchronously_FailsClosedToUnavailable()
        {
            var lookup = Substitute.For<IDiscountCodeLookup>();
            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns<Task<DiscountCodeValidation>>(_ => throw new InvalidOperationException("boom"));

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheLookupReturnsNothing_FailsClosedToUnavailable()
        {
            var lookup = LookupReturning(null);

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ValidateAsync_WhenTheCanonicalCodeIsBlank_FailsClosedToUnavailable(string canonical)
        {
            var lookup = LookupReturning(DiscountCodeValidation.Valid(canonical));

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
            Assert.Null(validation.DiscountCode);
        }

        [Fact]
        public async Task ValidateAsync_WhenTheCanonicalCodeIsLongerThanTheLimit_FailsClosedToUnavailable()
        {
            var lookup = LookupReturning(DiscountCodeValidation.Valid(new string('A', DiscountCodeLimits.MaxLength + 1)));

            var validation = await CreateValidator(lookup).ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, validation.Status);
        }

        [Fact]
        public async Task ValidateAsync_ForTheSameClientCodeAndToken_AsksTheLookupOnce()
        {
            var lookup = LookupReturning(DiscountCodeValidation.Valid("SUMMER25"));
            var validator = CreateValidator(lookup);

            var first = await validator.ValidateAsync(ClientId, "SUMMER25", "token");
            var second = await validator.ValidateAsync(ClientId, " summer25 ", "token");

            Assert.True(first.IsValid);
            Assert.True(second.IsValid);
            await lookup.Received(1).ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task ValidateAsync_ForConcurrentCallsWithTheSameIdentity_SharesOneLookup()
        {
            var completion = new TaskCompletionSource<DiscountCodeValidation>();
            var lookup = Substitute.For<IDiscountCodeLookup>();
            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>()).Returns(completion.Task);
            var validator = CreateValidator(lookup);

            var first = validator.ValidateAsync(ClientId, "SUMMER25", "token");
            var second = validator.ValidateAsync(ClientId, "SUMMER25", "token");

            completion.SetResult(DiscountCodeValidation.Valid("SUMMER25"));

            Assert.True((await first).IsValid);
            Assert.True((await second).IsValid);
            await lookup.Received(1).ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task ValidateAsync_ForADifferentClientCodeOrToken_AsksTheLookupAgain()
        {
            var lookup = LookupReturning(DiscountCodeValidation.Valid("SUMMER25"));
            var validator = CreateValidator(lookup);

            await validator.ValidateAsync(ClientId, "SUMMER25", "token");
            await validator.ValidateAsync(Guid.NewGuid(), "SUMMER25", "token");
            await validator.ValidateAsync(ClientId, "WINTER25", "token");
            await validator.ValidateAsync(ClientId, "SUMMER25", "other-token");

            await lookup.Received(4).ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task ValidateAsync_RemembersAFailureForTheRestOfTheRequest()
        {
            var lookup = Substitute.For<IDiscountCodeLookup>();
            lookup.ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>())
                .ThrowsAsync(new HttpRequestException("boom"));
            var validator = CreateValidator(lookup);

            var first = await validator.ValidateAsync(ClientId, "SUMMER25", "token");
            var second = await validator.ValidateAsync(ClientId, "SUMMER25", "token");

            Assert.Equal(DiscountCodeValidationStatus.Unavailable, first.Status);
            Assert.Equal(DiscountCodeValidationStatus.Unavailable, second.Status);
            await lookup.Received(1).ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>());
        }

        [Fact]
        public async Task ValidateAsync_DoesNotShareAnswersBetweenRequests()
        {
            var lookup = LookupReturning(DiscountCodeValidation.Valid("SUMMER25"));

            await CreateValidator(lookup).ValidateAsync(ClientId, "SUMMER25", "token");
            await CreateValidator(lookup).ValidateAsync(ClientId, "SUMMER25", "token");

            await lookup.Received(2).ValidateAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
