using Foundation.Pricing.DiscountCodes;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using Giuru.UnitTests.Helpers;
using NSubstitute;
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

        private static VerifiedDiscountCodePriceClientResolver CreateResolver(
            IPriceClientResolver inner,
            IDiscountCodeValidator validator,
            bool isGrulaConfigured = true)
        {
            return new VerifiedDiscountCodePriceClientResolver(
                inner,
                validator,
                new TestPricingSettings { GrulaAccessToken = isGrulaConfigured ? "token" : null });
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
        public async Task ResolveAsync_WithoutACode_DoesNotValidate(string discountCode)
        {
            var validator = DiscountCodeValidators.AlwaysValid();

            var priceClient = await CreateResolver(CreateInner(ClientId), validator).ResolveAsync(null, discountCode, "token");

            Assert.Null(priceClient.DiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }

        [Fact]
        public async Task ResolveAsync_WhenGrulaIsNotConfigured_DoesNotValidateAndDoesNotUseTheCode()
        {
            var validator = DiscountCodeValidators.AlwaysValid();

            var priceClient = await CreateResolver(CreateInner(ClientId), validator, isGrulaConfigured: false).ResolveAsync(null, "SUMMER25", "token");

            Assert.Null(priceClient.DiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default);
        }
    }
}
