using Foundation.Pricing.DiscountCodes;
using Foundation.Pricing.DomainModels;
using Foundation.Pricing.Services;
using Giuru.UnitTests.Helpers;
using NSubstitute;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    // The pricing seam: a code reaches Grula only when it exists and can be applied for the client.
    public class VerifiedDiscountCodePriceClientResolverTests
    {
        private static readonly Guid ClientId = Guid.NewGuid();

        private static (VerifiedDiscountCodePriceClientResolver Resolver, IPriceClientResolver Inner, IDiscountCodeValidator Validator) Create(
            bool enforcementEnabled = true,
            bool grulaConfigured = true,
            PriceClient innerClient = null,
            DiscountCodeValidation validation = null)
        {
            var inner = Substitute.For<IPriceClientResolver>();
            inner.ResolveAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(call => Task.FromResult(innerClient is null ? null : new PriceClient
                {
                    Id = innerClient.Id,
                    Name = innerClient.Name,
                    DiscountCode = call.ArgAt<string>(1)
                }));

            var validator = Substitute.For<IDiscountCodeValidator>();
            validator.ValidateAsync(Arg.Any<Guid?>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(Task.FromResult(validation ?? DiscountCodeValidation.Valid("SUMMER25")));

            var settings = new TestPricingSettings
            {
                DiscountCodeEnforcementEnabled = enforcementEnabled,
                GrulaAccessToken = grulaConfigured ? "token" : null
            };

            return (new VerifiedDiscountCodePriceClientResolver(inner, validator, settings), inner, validator);
        }

        private static PriceClient Client(Guid? id = null) => new() { Id = id ?? ClientId, Name = "Client" };

        [Fact]
        public async Task ResolveAsync_ForAValidCode_ReplacesItWithTheCanonicalSpelling()
        {
            var (resolver, _, validator) = Create(innerClient: Client());

            var priceClient = await resolver.ResolveAsync(ClientId, "summer25", "token");

            Assert.Equal("SUMMER25", priceClient.DiscountCode);
            await validator.Received(1).ValidateAsync(ClientId, "summer25", "token", Arg.Any<CancellationToken>());
        }

        [Theory]
        [InlineData(DiscountCodeValidationStatus.NotFound)]
        [InlineData(DiscountCodeValidationStatus.Disabled)]
        [InlineData(DiscountCodeValidationStatus.NotAssigned)]
        [InlineData(DiscountCodeValidationStatus.ClientUnknown)]
        [InlineData(DiscountCodeValidationStatus.NotApplicable)]
        public async Task ResolveAsync_ForACodeThatIsNotApplicable_ClearsIt(DiscountCodeValidationStatus status)
        {
            var (resolver, _, _) = Create(innerClient: Client(), validation: DiscountCodeValidation.Invalid(status));

            var priceClient = await resolver.ResolveAsync(ClientId, "summer25", "token");

            Assert.Null(priceClient.DiscountCode);
        }

        [Fact]
        public async Task ResolveAsync_WhenVerificationIsUnavailable_ClearsTheCode()
        {
            var (resolver, _, _) = Create(innerClient: Client(), validation: DiscountCodeValidation.Invalid(DiscountCodeValidationStatus.Unavailable));

            var priceClient = await resolver.ResolveAsync(ClientId, "summer25", "token");

            Assert.Null(priceClient.DiscountCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ResolveAsync_WithoutACode_DoesNotCallTheValidatorAndClearsTheInnerCode(string code)
        {
            var (resolver, _, validator) = Create(innerClient: Client());

            var priceClient = await resolver.ResolveAsync(ClientId, code, "token");

            Assert.Null(priceClient.DiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
        }

        [Fact]
        public async Task ResolveAsync_UsesTheInnerClientsIdRatherThanTheArgument()
        {
            // The buyer resolver takes the client from the claims and is called without an id.
            var claimsClientId = Guid.NewGuid();
            var (resolver, _, validator) = Create(innerClient: Client(claimsClientId));

            await resolver.ResolveAsync(null, "summer25", "token");

            await validator.Received(1).ValidateAsync(claimsClientId, "summer25", "token", Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task ResolveAsync_WhenTheInnerClientHasNoId_ValidatesWithANullClient()
        {
            var (resolver, _, validator) = Create(innerClient: new PriceClient { Id = null });

            await resolver.ResolveAsync(null, "summer25", "token");

            await validator.Received(1).ValidateAsync(null, "summer25", "token", Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task ResolveAsync_WhenTheInnerResolverReturnsNoClient_ReturnsNullWithoutValidating()
        {
            var (resolver, _, validator) = Create(innerClient: null);

            var priceClient = await resolver.ResolveAsync(ClientId, "summer25", "token");

            Assert.Null(priceClient);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
        }

        [Fact]
        public async Task ResolveAsync_WithEnforcementOn_GivesTheInnerResolverNoCode()
        {
            var (resolver, inner, _) = Create(innerClient: Client());

            await resolver.ResolveAsync(ClientId, "summer25", "token");

            await inner.Received(1).ResolveAsync(ClientId, null, "token");
        }

        [Fact]
        public async Task ResolveAsync_WithEnforcementOff_PassesTheCodeThroughWithoutValidating()
        {
            var (resolver, inner, validator) = Create(enforcementEnabled: false, innerClient: Client());

            var priceClient = await resolver.ResolveAsync(ClientId, "typed", "token");

            Assert.Equal("typed", priceClient.DiscountCode);
            await inner.Received(1).ResolveAsync(ClientId, "typed", "token");
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
        }

        [Fact]
        public async Task ResolveAsync_WhenGrulaIsNotConfigured_PassesTheCodeThroughWithoutValidating()
        {
            var (resolver, _, validator) = Create(grulaConfigured: false, innerClient: Client());

            var priceClient = await resolver.ResolveAsync(ClientId, "typed", "token");

            Assert.Equal("typed", priceClient.DiscountCode);
            await validator.DidNotReceiveWithAnyArgs().ValidateAsync(default, default, default, default);
        }
    }
}
