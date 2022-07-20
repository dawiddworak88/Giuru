using DownloadCenter.Api.ServicesModels.DownloadCenter;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace DownloadCenter.Api.Validators.DownloadCenter
{
    public class GetDownloadCenterCategoryFilesModelValidator : BaseServiceModelValidator<GetDownloadCenterFilesCategoryServiceModel>
    {
        public GetDownloadCenterCategoryFilesModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
