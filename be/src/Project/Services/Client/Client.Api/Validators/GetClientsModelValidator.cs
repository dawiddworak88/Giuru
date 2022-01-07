using Client.Api.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators
{
    public class GetClientsModelValidator : BasePagedServiceModelValidator<GetClientsServiceModel>
    {
        public GetClientsModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
        }
    }
}
