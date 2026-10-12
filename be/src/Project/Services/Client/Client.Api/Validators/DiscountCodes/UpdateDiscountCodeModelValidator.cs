using Client.Api.ServicesModels.DiscountCodes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.DiscountCodes
{
    public class UpdateDiscountCodeModelValidator : BaseServiceModelValidator<UpdateDiscountCodeServiceModel>
    {
        public UpdateDiscountCodeModelValidator()
        {
            this.RequireOrganisation();
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
            this.RuleFor(x => x.Version).BeAValidVersion();
            this.RuleFor(x => x.Code).BeAValidCode();
            this.RuleFor(x => x.Description).BeAValidDescription();
            this.RuleFor(x => x.ClientIds).BeValidClientIds();
        }
    }
}
