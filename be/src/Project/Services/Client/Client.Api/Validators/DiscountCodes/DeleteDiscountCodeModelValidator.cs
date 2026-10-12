using Client.Api.ServicesModels.DiscountCodes;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.DiscountCodes
{
    public class DeleteDiscountCodeModelValidator : BaseServiceModelValidator<DeleteDiscountCodeServiceModel>
    {
        public DeleteDiscountCodeModelValidator()
        {
            this.RequireOrganisation();
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
            this.RuleFor(x => x.Version).BeAValidVersion();
        }
    }
}
