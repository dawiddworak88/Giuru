using DownloadCenter.Api.ServicesModels.DownloadCenter;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace DownloadCenter.Api.Validators.DownloadCenter
{
    public class GetDownloadCenterFileModelValidator : BaseServiceModelValidator<GetDownloadCenterFileServiceModel>
    {
        public GetDownloadCenterFileModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
