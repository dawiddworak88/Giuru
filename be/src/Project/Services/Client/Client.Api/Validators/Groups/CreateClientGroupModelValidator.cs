using Client.Api.ServicesModels.Groups;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Groups
{
    public class CreateClientGroupModelValidator : BaseServiceModelValidator<CreateClientGroupServiceModel>
    {
        public CreateClientGroupModelValidator()
        {
            this.RuleFor(x => x.Name).NotNull().NotEmpty();
        }
    }
}
