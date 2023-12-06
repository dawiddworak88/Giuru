using Client.Api.ServicesModels.Fields;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Feilds
{
    public class UpdateClientFieldModelValidator : BaseServiceModelValidator<UpdateClientFieldServiceModel>
    {
        public UpdateClientFieldModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Type).NotNull().NotEmpty();
        }
    }
}
