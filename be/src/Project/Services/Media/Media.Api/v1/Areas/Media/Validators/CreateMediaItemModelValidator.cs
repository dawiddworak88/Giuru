using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.v1.Areas.Media.Models;

namespace Media.Api.v1.Areas.Media.Validators
{
    public class CreateMediaItemModelValidator : BaseAuthorizedServiceModelValidator<CreateMediaItemModel>
    {
        public CreateMediaItemModelValidator()
        {
            this.RuleFor(x => x.File).NotNull();
        }
    }
}
