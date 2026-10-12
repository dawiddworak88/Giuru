using Client.Api.ServicesModels.DiscountCodes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.DiscountCodes
{
    public class CreateDiscountCodeModelValidator : BaseServiceModelValidator<CreateDiscountCodeServiceModel>
    {
        public CreateDiscountCodeModelValidator()
        {
            this.RequireOrganisation();
            this.RuleFor(x => x.Code).BeAValidCode();
            this.RuleFor(x => x.Description).BeAValidDescription();
            this.RuleFor(x => x.ClientIds).BeValidClientIds();
        }
    }
}
