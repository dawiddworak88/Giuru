using Client.Api.ServicesModels.FieldOptions;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.FieldOptions
{
    public class CreateFieldOptionModelValidator : BaseServiceModelValidator<CreateFieldOptionServiceModel>
    {
        public CreateFieldOptionModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Value).NotNull().NotEmpty();
            RuleFor(x => x.FieldDefinitionId).NotNull().NotEmpty();
        }
    }
}
