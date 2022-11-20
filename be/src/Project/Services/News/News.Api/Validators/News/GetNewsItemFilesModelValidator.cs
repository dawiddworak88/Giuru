using FluentValidation;
using Foundation.Extensions.Validators;
using News.Api.ServicesModels.News;

namespace News.Api.Validators.News
{
    public class GetNewsItemFilesModelValidator : BasePagedServiceModelValidator<GetNewsItemFilesServiceModel>
    {
        public GetNewsItemFilesModelValidator()
        {
            this.RuleFor(x => x.Id).NotEmpty().NotNull();
        }
    }
}
