using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.OutletServiceModels;

namespace Inventory.Api.Validators.OutletValidators
{
    public class GetOutletModelValidator : BaseServiceModelValidator<GetOutletServiceModel>
    {
        public GetOutletModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
