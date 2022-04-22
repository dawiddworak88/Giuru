using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.OutletServiceModels;

namespace Inventory.Api.Validators.OutletValidators
{
    public class GetOutletsByIdsModelValidator : BaseServiceModelValidator<GetOutletsByIdsServiceModel>
    {
        public GetOutletsByIdsModelValidator()
        {
            RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
