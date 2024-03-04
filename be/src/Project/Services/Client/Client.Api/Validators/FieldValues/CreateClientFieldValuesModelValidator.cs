using Client.Api.ServicesModels.FieldValues;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.FieldValues
{
    public class CreateClientFieldValuesModelValidator : BaseServiceModelValidator<CreateClientFieldValuesServiceModel>
    {
        public CreateClientFieldValuesModelValidator()
        {
            RuleFor(x => x.ClientId).NotNull().NotEmpty();
            RuleFor(x => x.FieldsValues).NotNull().NotEmpty();
        }
    }
}
