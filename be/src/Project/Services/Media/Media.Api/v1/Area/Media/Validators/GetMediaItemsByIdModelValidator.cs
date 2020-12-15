using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.v1.Area.Media.Models;

namespace Media.Api.v1.Area.Media.Validators
{
    public class GetMediaItemsByIdModelValidator : BaseServiceModelValidator<GetMediaItemsByIdModel>
    {
        public GetMediaItemsByIdModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
