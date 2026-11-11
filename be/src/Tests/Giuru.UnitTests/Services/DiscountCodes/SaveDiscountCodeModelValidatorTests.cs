using Client.Api.ServicesModels.DiscountCodes;
using Client.Api.Validators.DiscountCodes;
using Foundation.Localization;
using Microsoft.Extensions.Localization;
using NSubstitute;
using System;

namespace Giuru.UnitTests.Services.DiscountCodes
{
    // The code text rule is part of the Seller-facing contract: letters, digits, '-' and '_', 3-50
    // characters. The controller trims the text before it reaches the validator.
    public class SaveDiscountCodeModelValidatorTests
    {
        [Theory]
        [InlineData("ABC")]
        [InlineData("summer-2026_A")]
        [InlineData("a1b2c3")]
        public void Validate_WithAllowedCode_IsValid(string code)
        {
            var result = CreateValidator().Validate(CreateModel(code));

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData("ab")]
        [InlineData("has space")]
        [InlineData("semi;colon")]
        [InlineData("dot.dot")]
        [InlineData("zażółć")]
        public void Validate_WithDisallowedCode_IsInvalid(string code)
        {
            var result = CreateValidator().Validate(CreateModel(code));

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Validate_WithFiftyCharacterCode_IsValidAndFiftyOneIsNot()
        {
            var validator = CreateValidator();

            Assert.True(validator.Validate(CreateModel(new string('A', 50))).IsValid);
            Assert.False(validator.Validate(CreateModel(new string('A', 51))).IsValid);
        }

        [Fact]
        public void Validate_WithoutOrganisation_IsInvalid()
        {
            var model = CreateModel("ABC");
            model.OrganisationId = null;

            var result = CreateValidator().Validate(model);

            Assert.False(result.IsValid);
        }

        private static SaveDiscountCodeModelValidator CreateValidator()
        {
            var localizer = Substitute.For<IStringLocalizer<ClientResources>>();
            localizer[Arg.Any<string>()].Returns(call => new LocalizedString(call.Arg<string>(), call.Arg<string>()));

            return new SaveDiscountCodeModelValidator(localizer);
        }

        private static SaveDiscountCodeServiceModel CreateModel(string code)
        {
            return new SaveDiscountCodeServiceModel
            {
                Code = code,
                OrganisationId = Guid.NewGuid()
            };
        }
    }
}
