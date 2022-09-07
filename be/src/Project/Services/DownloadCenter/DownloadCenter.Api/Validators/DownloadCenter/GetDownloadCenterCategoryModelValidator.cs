using DownloadCenter.Api.ServicesModels.DownloadCenter;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace DownloadCenter.Api.Validators.DownloadCenter
{
    public class GetDownloadCenterCategoryModelValidator : BaseServiceModelValidator<GetDownloadCenterCategoryServiceModel>
    {
        public GetDownloadCenterCategoryModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
