using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.OutletServices;

namespace Inventory.Api.Validators.OutletValidators
{
    public class DeleteOutletModelValidator : BaseServiceModelValidator<DeleteOutletServiceModel>
    {
        public DeleteOutletModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
