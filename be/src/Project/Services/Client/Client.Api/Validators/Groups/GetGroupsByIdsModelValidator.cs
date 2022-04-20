using Client.Api.ServicesModels.Groups;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Groups
{
    public class GetGroupsByIdsModelValidator : BaseServiceModelValidator<GetGroupsByIdsServiceModel>
    {
        public GetGroupsByIdsModelValidator()
        {
            this.RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
