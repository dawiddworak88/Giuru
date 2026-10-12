using Client.Api.ServicesModels.DiscountCodes;
using Client.Api.Validators.DiscountCodes;
using System;
using System.Linq;

namespace Giuru.UnitTests.Services.ClientApi
{
    public class DiscountCodeValidatorsTests
    {
        private static readonly Guid OrganisationId = Guid.NewGuid();
        private static readonly string Version = Convert.ToBase64String(new byte[] { 0, 0, 0, 0, 0, 0, 7, 209 });

        [Fact]
        public void Create_WithACodeAnEmptyAssignmentListAndAnOrganisation_IsValid()
        {
            var result = new CreateDiscountCodeModelValidator().Validate(new CreateDiscountCodeServiceModel
            {
                Code = "SUMMER25",
                ClientIds = Array.Empty<Guid>(),
                OrganisationId = OrganisationId
            });

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_WithABlankCode_IsInvalid(string code)
        {
            var result = new CreateDiscountCodeModelValidator().Validate(new CreateDiscountCodeServiceModel
            {
                Code = code,
                ClientIds = Array.Empty<Guid>(),
                OrganisationId = OrganisationId
            });

            Assert.False(result.IsValid);
        }

        [Theory]
        [InlineData(64, true)]
        [InlineData(65, false)]
        public void Create_AppliesTheCodeLimitToTheTrimmedText(int length, bool expectedValid)
        {
            var result = new CreateDiscountCodeModelValidator().Validate(new CreateDiscountCodeServiceModel
            {
                Code = $"  {new string('A', length)}  ",
                ClientIds = Array.Empty<Guid>(),
                OrganisationId = OrganisationId
            });

            Assert.Equal(expectedValid, result.IsValid);
        }

        [Theory]
        [InlineData(256, true)]
        [InlineData(257, false)]
        public void Create_LimitsTheDescription(int length, bool expectedValid)
        {
            var result = new CreateDiscountCodeModelValidator().Validate(new CreateDiscountCodeServiceModel
            {
                Code = "SUMMER25",
                Description = new string('d', length),
                ClientIds = Array.Empty<Guid>(),
                OrganisationId = OrganisationId
            });

            Assert.Equal(expectedValid, result.IsValid);
        }

        [Fact]
        public void Create_WithoutAnAssignmentList_IsInvalid_ButAnEmptyOneIsValid()
        {
            var withoutList = new CreateDiscountCodeModelValidator().Validate(new CreateDiscountCodeServiceModel
            {
                Code = "SUMMER25",
                ClientIds = null,
                OrganisationId = OrganisationId
            });

            Assert.False(withoutList.IsValid);
        }

        [Fact]
        public void Create_WithAnEmptyClientId_IsInvalid()
        {
            var result = new CreateDiscountCodeModelValidator().Validate(new CreateDiscountCodeServiceModel
            {
                Code = "SUMMER25",
                ClientIds = new[] { Guid.NewGuid(), Guid.Empty },
                OrganisationId = OrganisationId
            });

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Create_WithoutAnOrganisation_IsInvalid()
        {
            var withoutOrganisation = new CreateDiscountCodeModelValidator().Validate(new CreateDiscountCodeServiceModel { Code = "SUMMER25", ClientIds = Array.Empty<Guid>() });
            var withEmptyOrganisation = new CreateDiscountCodeModelValidator().Validate(new CreateDiscountCodeServiceModel { Code = "SUMMER25", ClientIds = Array.Empty<Guid>(), OrganisationId = Guid.Empty });

            Assert.False(withoutOrganisation.IsValid);
            Assert.False(withEmptyOrganisation.IsValid);
        }

        [Fact]
        public void Update_WithAnIdAVersionAndEverythingStatedAgain_IsValid()
        {
            var result = new UpdateDiscountCodeModelValidator().Validate(new UpdateDiscountCodeServiceModel
            {
                Id = Guid.NewGuid(),
                Version = Version,
                Code = "SUMMER25",
                ClientIds = Array.Empty<Guid>(),
                OrganisationId = OrganisationId
            });

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("not base64!")]
        [InlineData("AAAA")]
        public void Update_WithoutAUsableVersion_IsInvalid(string version)
        {
            var result = new UpdateDiscountCodeModelValidator().Validate(new UpdateDiscountCodeServiceModel
            {
                Id = Guid.NewGuid(),
                Version = version,
                Code = "SUMMER25",
                ClientIds = Array.Empty<Guid>(),
                OrganisationId = OrganisationId
            });

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Update_WithoutAnId_IsInvalid()
        {
            var result = new UpdateDiscountCodeModelValidator().Validate(new UpdateDiscountCodeServiceModel
            {
                Version = Version,
                Code = "SUMMER25",
                ClientIds = Array.Empty<Guid>(),
                OrganisationId = OrganisationId
            });

            Assert.False(result.IsValid);
        }

        [Fact]
        public void Delete_NeedsAnIdAVersionAndAnOrganisation()
        {
            var validator = new DeleteDiscountCodeModelValidator();

            Assert.True(validator.Validate(new DeleteDiscountCodeServiceModel { Id = Guid.NewGuid(), Version = Version, OrganisationId = OrganisationId }).IsValid);
            Assert.False(validator.Validate(new DeleteDiscountCodeServiceModel { Version = Version, OrganisationId = OrganisationId }).IsValid);
            Assert.False(validator.Validate(new DeleteDiscountCodeServiceModel { Id = Guid.NewGuid(), OrganisationId = OrganisationId }).IsValid);
            Assert.False(validator.Validate(new DeleteDiscountCodeServiceModel { Id = Guid.NewGuid(), Version = Version }).IsValid);
        }

        [Fact]
        public void Get_NeedsAnIdAndAnOrganisation()
        {
            var validator = new GetDiscountCodeModelValidator();

            Assert.True(validator.Validate(new GetDiscountCodeServiceModel { Id = Guid.NewGuid(), OrganisationId = OrganisationId }).IsValid);
            Assert.False(validator.Validate(new GetDiscountCodeServiceModel { OrganisationId = OrganisationId }).IsValid);
            Assert.False(validator.Validate(new GetDiscountCodeServiceModel { Id = Guid.NewGuid() }).IsValid);
        }

        [Fact]
        public void ListAndOptions_NeedAnOrganisation()
        {
            Assert.True(new GetDiscountCodesModelValidator().Validate(new GetDiscountCodesServiceModel { OrganisationId = OrganisationId }).IsValid);
            Assert.False(new GetDiscountCodesModelValidator().Validate(new GetDiscountCodesServiceModel()).IsValid);
            Assert.True(new GetDiscountCodeClientOptionsModelValidator().Validate(new GetDiscountCodeClientOptionsServiceModel { OrganisationId = OrganisationId }).IsValid);
            Assert.False(new GetDiscountCodeClientOptionsModelValidator().Validate(new GetDiscountCodeClientOptionsServiceModel { OrganisationId = Guid.Empty }).IsValid);
        }

        [Fact]
        public void Validate_NeedsACodeAClientAndAnOrganisation()
        {
            var validator = new ValidateDiscountCodeModelValidator();

            Assert.True(validator.Validate(new ValidateDiscountCodeServiceModel { Code = "SUMMER25", ClientId = Guid.NewGuid(), OrganisationId = OrganisationId }).IsValid);
            Assert.False(validator.Validate(new ValidateDiscountCodeServiceModel { Code = " ", ClientId = Guid.NewGuid(), OrganisationId = OrganisationId }).IsValid);
            Assert.False(validator.Validate(new ValidateDiscountCodeServiceModel { Code = new string('A', 65), ClientId = Guid.NewGuid(), OrganisationId = OrganisationId }).IsValid);
            Assert.False(validator.Validate(new ValidateDiscountCodeServiceModel { Code = "SUMMER25", ClientId = null, OrganisationId = OrganisationId }).IsValid);
            Assert.False(validator.Validate(new ValidateDiscountCodeServiceModel { Code = "SUMMER25", ClientId = Guid.Empty, OrganisationId = OrganisationId }).IsValid);
            Assert.False(validator.Validate(new ValidateDiscountCodeServiceModel { Code = "SUMMER25", ClientId = Guid.NewGuid() }).IsValid);
            Assert.False(validator.Validate(new ValidateDiscountCodeServiceModel { Code = "SUMMER25", ClientId = Guid.NewGuid(), OrganisationId = Guid.Empty }).IsValid);
        }

        [Fact]
        public void TryDecodeVersion_AcceptsOnlyAnEightByteBase64RowVersion()
        {
            Assert.True(DiscountCodeRules.TryDecodeVersion(Version, out var decoded));
            Assert.Equal(8, decoded.Length);
            Assert.Equal(new byte[] { 0, 0, 0, 0, 0, 0, 7, 209 }, decoded.ToArray());

            Assert.False(DiscountCodeRules.TryDecodeVersion(null, out _));
            Assert.False(DiscountCodeRules.TryDecodeVersion("   ", out _));
            Assert.False(DiscountCodeRules.TryDecodeVersion("%%%", out _));
            Assert.False(DiscountCodeRules.TryDecodeVersion(Convert.ToBase64String(new byte[9]), out _));
        }
    }
}
