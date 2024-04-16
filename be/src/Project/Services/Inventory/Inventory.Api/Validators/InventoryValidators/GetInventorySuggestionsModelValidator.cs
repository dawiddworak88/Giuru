using FluentValidation;
using Foundation.Extensions.Validators;
using Inventory.Api.ServicesModels.InventoryServiceModels;

namespace Inventory.Api.Validators.InventoryValidators
{
    public class GetInventorySuggestionsModelValidator : BaseServiceModelValidator<GetInventorySuggestionsServiceModel>
    {
        public GetInventorySuggestionsModelValidator() 
        { 
            RuleFor(x => x.SearchTerm).NotNull().NotEmpty();
            RuleFor(x => x.SuggestionsCount).LessThanOrEqualTo(100);
        }
    }
}
