using DownloadCenter.Api.ServicesModels.DownloadCenter;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace DownloadCenter.Api.Validators.DownloadCenter
{
    public class CreateDownloadCenterFileModelValidator : BaseServiceModelValidator<CreateDownloadCenterFileServiceModel>
    {
        public CreateDownloadCenterFileModelValidator()
        {
            this.RuleFor(x => x.CategoriesIds).NotEmpty().NotNull();
            this.RuleFor(x => x.Files).NotEmpty().NotNull();
        }
    }
}
