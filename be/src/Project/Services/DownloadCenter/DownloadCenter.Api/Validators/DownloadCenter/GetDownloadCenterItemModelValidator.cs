using DownloadCenter.Api.ServicesModels.DownloadCenter;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace DownloadCenter.Api.Validators.DownloadCenter
{
    public class GetDownloadCenterItemModelValidator : BaseServiceModelValidator<GetDownloadCenterItemServiceModel>
    {
        public GetDownloadCenterItemModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
