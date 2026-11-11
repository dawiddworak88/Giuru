using Client.Api.ServicesModels.DiscountCodes;
using FluentValidation;
using Foundation.Extensions.Validators;
using Foundation.Localization;
using Microsoft.Extensions.Localization;

namespace Client.Api.Validators.DiscountCodes
{
    public class SaveDiscountCodeModelValidator : BaseServiceModelValidator<SaveDiscountCodeServiceModel>
    {
        public const string CodePattern = "^[A-Za-z0-9_-]{3,50}$";

        public SaveDiscountCodeModelValidator(IStringLocalizer<ClientResources> clientLocalizer)
        {
            this.RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
            this.RuleFor(x => x.Code)
                .NotEmpty()
                .WithMessage(clientLocalizer.GetString("DiscountCodeFormatInvalid"))
                .Matches(CodePattern)
                .WithMessage(clientLocalizer.GetString("DiscountCodeFormatInvalid"));
        }
    }
}
