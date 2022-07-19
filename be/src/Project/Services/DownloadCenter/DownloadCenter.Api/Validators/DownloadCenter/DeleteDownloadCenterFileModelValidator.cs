using DownloadCenter.Api.ServicesModels.DownloadCenter;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace DownloadCenter.Api.Validators.DownloadCenter
{
    public class DeleteDownloadCenterFileModelValidator : BaseServiceModelValidator<DeleteDownloadCenterFileServiceModel>
    {
        public DeleteDownloadCenterFileModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
