using Client.Api.ServicesModels;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators
{
    public class GeClientsByIdsModelValidator : BaseServiceModelValidator<GetClientsByIdsServiceModel>
    {
        public GeClientsByIdsModelValidator()
        {
            RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
