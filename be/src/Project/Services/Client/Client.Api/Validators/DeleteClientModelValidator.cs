using Client.Api.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators
{
    public class DeleteClientModelValidator : BaseServiceModelValidator<DeleteClientServiceModel>
    {
        public DeleteClientModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull();
        }
    }
}
