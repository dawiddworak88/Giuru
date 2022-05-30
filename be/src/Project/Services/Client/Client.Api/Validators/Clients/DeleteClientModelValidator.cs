using Client.Api.ServicesModels.Clients;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Clients
{
    public class DeleteClientModelValidator : BaseServiceModelValidator<DeleteClientServiceModel>
    {
        public DeleteClientModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull();
        }
    }
}
