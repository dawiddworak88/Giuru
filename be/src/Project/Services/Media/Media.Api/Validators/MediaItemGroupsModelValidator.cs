using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.ServicesModels;

namespace Media.Api.Validators
{
    public class MediaItemGroupsModelValidator : BaseServiceModelValidator<MediaItemGroupsServiceModel>
    {
        public MediaItemGroupsModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
            this.RuleFor(x => x.GroupIds).NotEmpty().NotNull();
        }
    }
}
