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
            this.RequireOrganisation();
            this.RuleFor(x => x.Code).BeAValidCode();
            this.RuleFor(x => x.ClientId).NotNull().Must(x => x.HasValue && x.Value != Guid.Empty);
        }
    }
}
