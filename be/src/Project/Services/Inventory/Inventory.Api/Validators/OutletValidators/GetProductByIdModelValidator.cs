using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.OutletServiceModels;

namespace Inventory.Api.Validators.OutletValidators
{
    public class GetProductByIdModelValidator : BaseServiceModelValidator<GetOutletByProductIdServiceModel>
    {
        public GetProductByIdModelValidator()
        {
            this.RuleFor(x => x.ProductId.Value).NotNull().NotEmpty();
        }
    }
}
