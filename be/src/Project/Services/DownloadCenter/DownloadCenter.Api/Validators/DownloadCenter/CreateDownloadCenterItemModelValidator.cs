using DownloadCenter.Api.ServicesModels.DownloadCenter;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace DownloadCenter.Api.Validators.DownloadCenter
{
    public class CreateDownloadCenterItemModelValidator : BaseServiceModelValidator<CreateDownloadCenterItemServiceModel>
    {
        public CreateDownloadCenterItemModelValidator()
        {
            this.RuleFor(x => x.CategoryId).NotEmpty().NotNull();
        }
    }
}
