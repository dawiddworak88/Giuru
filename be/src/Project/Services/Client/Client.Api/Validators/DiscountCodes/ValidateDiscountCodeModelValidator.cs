using Client.Api.ServicesModels.DiscountCodes;
using FluentValidation;
using Foundation.Extensions.Validators;
using System;

namespace Client.Api.Validators.DiscountCodes
{
    public class ValidateDiscountCodeModelValidator : BaseServiceModelValidator<ValidateDiscountCodeServiceModel>
    {
        public ValidateDiscountCodeModelValidator()
        {
            this.RuleFor(x => x.Code).NotNull().NotEmpty().Must(x => string.IsNullOrWhiteSpace(x) is false);
            this.RuleFor(x => x.Code).MaximumLength(DiscountCodesConstants.CodeMaxLength);
            this.RuleFor(x => x.ClientId).NotNull().Must(x => x.HasValue && x.Value != Guid.Empty);
        }
    }
}
