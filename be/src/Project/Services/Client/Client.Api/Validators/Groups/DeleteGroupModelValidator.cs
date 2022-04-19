using Client.Api.ServicesModels.Groups;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Groups
{
    public class DeleteGroupModelValidator : BaseServiceModelValidator<DeleteGroupServiceModel>
    {
        public DeleteGroupModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
