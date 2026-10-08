using Client.Api.ServicesModels.DiscountCodes;
using FluentValidation;
using Foundation.Extensions.Validators;
using System;

namespace Client.Api.Validators.DiscountCodes
{
    public class CreateDiscountCodeModelValidator : BaseServiceModelValidator<CreateDiscountCodeServiceModel>
    {
        public CreateDiscountCodeModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotNull().Must(x => x.HasValue && x.Value != Guid.Empty);
            this.RuleFor(x => x.Code).NotNull().NotEmpty().Must(x => string.IsNullOrWhiteSpace(x) is false);
            this.RuleFor(x => x.Code).MaximumLength(DiscountCodesConstants.CodeMaxLength);
            this.RuleFor(x => x.Description).MaximumLength(DiscountCodesConstants.DescriptionMaxLength);
        }
    }
}
