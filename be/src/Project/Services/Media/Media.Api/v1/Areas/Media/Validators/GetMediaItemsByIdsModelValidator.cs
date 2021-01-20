using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.v1.Areas.Media.Models;

namespace Media.Api.v1.Areas.Media.Validators
{
    public class GetMediaItemsByIdsModelValidator : BaseServiceModelValidator<GetMediaItemsByIdsModel>
    {
        public GetMediaItemsByIdsModelValidator()
        {
            this.RuleFor(x => x.Ids).NotNull().NotEmpty();
        }
    }
}
