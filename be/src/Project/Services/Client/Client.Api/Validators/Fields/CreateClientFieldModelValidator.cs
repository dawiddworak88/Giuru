using Client.Api.ServicesModels.Fields;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Fields
{
    public class CreateClientFieldModelValidator : BaseServiceModelValidator<CreateClientFieldServiceModel>
    {
        public CreateClientFieldModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Type).NotNull().NotEmpty();
        }
    }
}
