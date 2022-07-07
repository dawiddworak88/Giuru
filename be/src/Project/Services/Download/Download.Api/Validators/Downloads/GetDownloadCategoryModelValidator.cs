using Download.Api.ServicesModels.Downloads;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Download.Api.Validators.Downloads
{
    public class GetDownloadCategoryModelValidator : BaseServiceModelValidator<GetDownloadCategoryServiceModel>
    {
        public GetDownloadCategoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
