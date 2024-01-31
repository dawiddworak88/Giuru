using Client.Api.ServicesModels.Fields;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Fields
{
    public class GetClientFieldDefinitionModelValidator : BaseServiceModelValidator<GetClientFieldDefinitionServiceModel>
    {
        public GetClientFieldDefinitionModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
