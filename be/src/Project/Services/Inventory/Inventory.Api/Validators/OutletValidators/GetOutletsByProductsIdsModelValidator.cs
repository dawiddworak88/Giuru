using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.OutletServiceModels;

namespace Inventory.Api.Validators.OutletValidators
{
    public class GetOutletsByProductsIdsModelValidator : BaseServiceModelValidator<GetOutletsByProductsIdsServiceModel>
    {
        public GetOutletsByProductsIdsModelValidator()
        {
            RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
