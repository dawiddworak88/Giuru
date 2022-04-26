using Client.Api.ServicesModels.Groups;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.Groups
{
    public class GetClientGroupModelValidator : BaseServiceModelValidator<GetClientGroupServiceModel>
    {
        public GetClientGroupModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
