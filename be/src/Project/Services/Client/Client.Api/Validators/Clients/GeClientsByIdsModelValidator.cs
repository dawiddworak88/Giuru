using Client.Api.ServicesModels.Clients;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Clients
{
    public class GeClientsByIdsModelValidator : BaseServiceModelValidator<GetClientsByIdsServiceModel>
    {
        public GeClientsByIdsModelValidator()
        {
            RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
