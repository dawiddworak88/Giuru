using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.ServicesModels;

namespace Media.Api.Validators
{
    public class GetMediaItemsByIdsModelValidator : BaseServiceModelValidator<GetMediaItemsByIdsServiceModel>
    {
        public GetMediaItemsByIdsModelValidator()
        {
            this.RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
