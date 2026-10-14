using Foundation.Pricing.DiscountCodes;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using Giuru.UnitTests.Helpers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    public class VerifiedDiscountCodePriceClientResolverTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        private static IPriceClientResolver CreateInner(Guid? resolvedClientId)
        {
            var inner = Substitute.For<IPriceClientResolver>();

            // The inner resolver echoes the code it is given, so a test sees whether the decorator held it back.
            inner.ResolveAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(call => Task.FromResult(new PriceClient { Id = resolvedClientId, Name = "Acme", DiscountCode = call.ArgAt<string>(1) }));

            return inner;
        }

        private static IStoredDiscountCodeReader StoredCode(string storedDiscountCode)
        {
            var reader = Substitute.For<IStoredDiscountCodeReader>();
            reader.ReadAsync(Arg.Any<Guid?>(), Arg.Any<string>()).Returns(Task.FromResult(storedDiscountCode));

            return reader;
        }

        private static VerifiedDiscountCodePriceClientResolver CreateResolver(
            IPriceClientResolver inner,
            IDiscountCodeValidator validator,
            bool isGrulaConfigured = true,
            IStoredDiscountCodeReader storedDiscountCodeReader = null)
        {
            return new VerifiedDiscountCodePriceClientResolver(
                inner,
                validator,
                storedDiscountCodeReader ?? StoredCode(null),
                new TestPricingSettings { GrulaAccessToken = isGrulaConfigured ? "token" : null },
                Substitute.For<ILogger<VerifiedDiscountCodePriceClientResolver>>());
        }

        [Fact]
        public async Task ResolveAsync_WhenTheCodeIsApplicable_UsesTheCanonicalSpelling()
        {
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(ClientId, "summer25", "token").Returns(Task.FromResult(DiscountCodeValidation.Valid("SUMMER25")));

            var priceClient = await CreateResolver(CreateInner(ClientId), validator).ResolveAsync(null, "summer25", "token");

            Assert.Equal("SUMMER25", priceClient.DiscountCode);
            Assert.Equal(ClientId, priceClient.Id);
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.NotFound)]
        [InlineData(DiscountCodeValidationStatus.Disabled)]
        [InlineData(DiscountCodeValidationStatus.NotAssigned)]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown)]
        [InlineData(DiscountCodeValidationStatus.NotApplicable)]
        [InlineData(DiscountCodeValidationStatus.Unavailable)]
        public async Task ResolveAsync_WhenTheCodeIsNotApplicableOrCannotBeVerified_LeavesItOut(DiscountCodeValidationStatus status)
        {
            var priceClient = await CreateResolver(CreateInner(ClientId), DiscountCodeValidators.Returning(status)).ResolveAsync(null, "SUMMER25", "token");

            Assert.Null(priceClient.DiscountCode);
            Assert.Equal("Acme", priceClient.Name);
        }

        [Fact]
        public async Task ResolveAsync_AlwaysAsksTheInnerResolverWithoutTheCode()
        {
            var inner = CreateInner(ClientId);

            await CreateResolver(inner, DiscountCodeValidators.AlwaysValid()).ResolveAsync(ClientId, "SUMMER25", "token");

            await inner.Received(1).ResolveAsync(ClientId, null, "token");
        }

        [Fact]
        public async Task ResolveAsync_ValidatesTheClientTheInnerResolverResolved_NotTheCallersArgument()
        {
            var resolvedClientId = Guid.NewGuid();
            var validator = DiscountCodeValidators.AlwaysValid();

            await CreateResolver(CreateInner(resolvedClientId), validator).ResolveAsync(null, "SUMMER25", "token");

            await validator.Received(1).ValidateAsync(resolvedClientId, "SUMMER25", "token");
        }

        [Fact]
        public async Task ResolveAsync_ChecksTheStoredCodeBindingForTheResolvedClient()
        {
            var resolvedClientId = Guid.NewGuid();
            var reader = StoredCode("SUMMER25");

            await CreateResolver(CreateInner(resolvedClientId), DiscountCodeValidators.AlwaysValid(), storedDiscountCodeReader: reader)
                .ResolveAsync(ClientId, "SUMMER25", "token");

            await reader.Received(1).ReadAsync(resolvedClientId, "token");
        }

        [Fact]
        public async Task ResolveAsync_WhenNoClientWasResolved_LeavesTheCodeOut()
        {
            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(null, "SUMMER25", "token")
                .Returns(Task.FromResult(DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.ClientUnknown)));

            var priceClient = await CreateResolver(CreateInner(null), validator).ResolveAsync(null, "SUMMER25", "token");

            Assert.Null(priceClient.DiscountCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ResolveAsync_WithoutACode_DoesNotValidateOrReadTheBasket(string discountCode)
        {
            var validator = DiscountCodeValidators.AlwaysValid();
            var reader = StoredCode("SUMMER25");

            var priceClient = await CreateResolver(CreateInner(ClientId), validator, storedDiscountCodeReader: reader).ResolveAsync(null, discountCode, "token");

            Assert.Null(priceClient.DiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
            await reader.DidNotReceiveWithAnyArgs().ReadAsync(default, default);
        }

        [Fact]
        public async Task ResolveAsync_WhenGrulaIsNotConfigured_DoesNotValidateAndDoesNotUseTheCode()
        {
            var validator = DiscountCodeValidators.AlwaysValid();
            var reader = StoredCode("SUMMER25");

            var priceClient = await CreateResolver(CreateInner(ClientId), validator, isGrulaConfigured: false, storedDiscountCodeReader: reader).ResolveAsync(null, "SUMMER25", "token");

            Assert.Null(priceClient.DiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
            await reader.DidNotReceiveWithAnyArgs().ReadAsync(default, default);
        }

        [Theory]
        [InlineData("SUMMER25")]
        [InlineData(" summer25 ")]
        public async Task ResolveAsync_WhenTheCodeIsTheStoredOne_UsesItWithoutValidation_EvenAfterItWasRevoked(string requested)
        {
            // The basket keeps a stored code even once it is revoked, so the product page has to price it the same way.
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.Disabled);

            var priceClient = await CreateResolver(CreateInner(ClientId), validator, storedDiscountCodeReader: StoredCode(" SUMMER25 "))
                .ResolveAsync(null, requested, "token");

            Assert.Equal("SUMMER25", priceClient.DiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Fact]
        public async Task ResolveAsync_ForAnAppThatRevalidatesStoredCodes_ValidatesEveryCodeForTheClientBeingPriced()
        {
            // The seller's basket is not bound to a client, so a code stored on it is validated for every client it is priced for.
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.NotAssigned);

            var priceClient = await CreateResolver(CreateInner(ClientId), validator, storedDiscountCodeReader: new NoStoredDiscountCodeReader())
                .ResolveAsync(ClientId, "STORED25", "token");

            Assert.Null(priceClient.DiscountCode);
            await validator.Received(1).ValidateAsync(ClientId, "STORED25", "token");
        }

        [Fact]
        public async Task ResolveAsync_WhenTheCodeIsNotTheStoredOne_ValidatesIt()
        {
            var validator = DiscountCodeValidators.Returning(DiscountCodeValidationStatus.NotAssigned);

            var priceClient = await CreateResolver(CreateInner(ClientId), validator, storedDiscountCodeReader: StoredCode("WINTER10"))
                .ResolveAsync(null, "SUMMER25", "token");

            Assert.Null(priceClient.DiscountCode);
            await validator.Received(1).ValidateAsync(ClientId, "SUMMER25", "token");
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.Valid, "SUMMER25")]
        [InlineData(DiscountCodeValidationStatus.Disabled, null)]
        public async Task ResolveAsync_WhenTheStoredCodeCannotBeRead_ValidatesTheCodeInstead(DiscountCodeValidationStatus status, string expected)
        {
            var reader = Substitute.For<IStoredDiscountCodeReader>();
            reader.ReadAsync(Arg.Any<Guid?>(), Arg.Any<string>()).ThrowsAsync(new InvalidOperationException("Basket.Api is down."));
            var validator = status is DiscountCodeValidationStatus.Valid
                ? DiscountCodeValidators.AlwaysValid()
                : DiscountCodeValidators.Returning(status);

            var priceClient = await CreateResolver(CreateInner(ClientId), validator, storedDiscountCodeReader: reader)
                .ResolveAsync(null, "SUMMER25", "token");

            Assert.Equal(expected, priceClient.DiscountCode);
            await validator.Received(1).ValidateAsync(ClientId, "SUMMER25", "token");
        }
    }
}
