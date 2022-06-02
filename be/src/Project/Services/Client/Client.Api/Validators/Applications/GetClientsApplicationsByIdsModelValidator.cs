using Client.Api.ServicesModels.Applications;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Applications
{
    public class GetClientsApplicationsByIdsModelValidator : BaseServiceModelValidator<GetClientsApplicationsByIdsServiceModel>
    {
        public GetClientsApplicationsByIdsModelValidator()
        {
            this.RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
