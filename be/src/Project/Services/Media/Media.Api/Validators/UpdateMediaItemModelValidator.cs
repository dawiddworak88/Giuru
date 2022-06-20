using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.ServicesModels;

namespace Media.Api.Validators
{
    public class UpdateMediaItemModelValidator : BaseAuthorizedServiceModelValidator<UpdateMediaItemServiceModel>
    {
        public UpdateMediaItemModelValidator()
        {
            this.RuleFor(x => x.File).NotNull().NotEmpty();
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.OrganisationId).NotNull().NotEmpty();
        }
    }
}
