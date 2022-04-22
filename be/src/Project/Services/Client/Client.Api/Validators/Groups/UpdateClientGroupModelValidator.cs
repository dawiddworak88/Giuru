using Client.Api.ServicesModels.Groups;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Groups
{
    public class UpdateClientGroupModelValidator : BaseServiceModelValidator<UpdateClientGroupServiceModel>
    {
        public UpdateClientGroupModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
