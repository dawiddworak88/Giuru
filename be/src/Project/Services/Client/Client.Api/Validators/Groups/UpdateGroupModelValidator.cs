using Client.Api.ServicesModels.Groups;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Groups
{
    public class UpdateGroupModelValidator : BaseServiceModelValidator<UpdateGroupServiceModel>
    {
        public UpdateGroupModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.Name).NotEmpty().NotNull();
        }
    }
}
