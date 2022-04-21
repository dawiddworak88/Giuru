using Client.Api.ServicesModels.Groups;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Groups
{
    public class DeleteClientGroupModelValidator : BaseServiceModelValidator<DeleteClientGroupServiceModel>
    {
        public DeleteClientGroupModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
