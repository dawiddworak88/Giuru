using DownloadCenter.Api.ServicesModels.DownloadCenter;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace DownloadCenter.Api.Validators.DownloadCenter
{
    public class UpdateDownloadCenterItemModelValidator : BaseServiceModelValidator<UpdateDownloadCenterItemServiceModel>
    {
        public UpdateDownloadCenterItemModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
            this.RuleFor(x => x.CategoriesIds).NotEmpty().NotNull();
            this.RuleFor(x => x.Files).NotEmpty().NotNull();
        }
    }
}
