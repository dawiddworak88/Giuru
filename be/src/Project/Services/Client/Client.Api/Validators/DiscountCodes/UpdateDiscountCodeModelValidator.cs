using Client.Api.ServicesModels.DiscountCodes;
using FluentValidation;
using Foundation.Extensions.Validators;
using System;

namespace Client.Api.Validators.DiscountCodes
{
    public class UpdateDiscountCodeModelValidator : BaseServiceModelValidator<UpdateDiscountCodeServiceModel>
    {
        public UpdateDiscountCodeModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotNull().Must(x => x.HasValue && x.Value != Guid.Empty);
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.Description).MaximumLength(DiscountCodesConstants.DescriptionMaxLength);
        }
    }
}
