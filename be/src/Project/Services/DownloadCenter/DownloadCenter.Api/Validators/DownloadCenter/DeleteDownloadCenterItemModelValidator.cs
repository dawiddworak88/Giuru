using DownloadCenter.Api.ServicesModels.DownloadCenter;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace DownloadCenter.Api.Validators.DownloadCenter
{
    public class DeleteDownloadCenterItemModelValidator : BaseServiceModelValidator<DeleteDownloadCenterItemServiceModel>
    {
        public DeleteDownloadCenterItemModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
