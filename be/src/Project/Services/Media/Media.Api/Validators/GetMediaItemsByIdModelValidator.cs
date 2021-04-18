using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.ServicesModels;

namespace Media.Api.Validators
{
    public class GetMediaItemsByIdModelValidator : BaseServiceModelValidator<GetMediaItemsByIdServiceModel>
    {
        public GetMediaItemsByIdModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
