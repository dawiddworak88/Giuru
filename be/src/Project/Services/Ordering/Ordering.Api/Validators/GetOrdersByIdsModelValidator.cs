using FluentValidation;
using Foundation.Extensions.Validators;
using Ordering.Api.ServicesModels;

namespace Ordering.Api.Validators
{
    public class GetOrdersByIdsModelValidator : BaseServiceModelValidator<GetOrdersByIdsServiceModel>
    {
        public GetOrdersByIdsModelValidator()
        {
            this.RuleFor(x => x.OrganisationId).NotEmpty().NotNull();
            this.RuleFor(x => x.Ids).NotEmpty().NotNull();
        }
    }
}
