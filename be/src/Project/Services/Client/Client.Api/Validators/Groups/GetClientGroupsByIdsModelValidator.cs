using Client.Api.ServicesModels.Groups;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Groups
{
    public class GetClientGroupsByIdsModelValidator : BaseServiceModelValidator<GetClientGroupsByIdsServiceModel>
    {
        public GetClientGroupsByIdsModelValidator()
        {
            this.RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
