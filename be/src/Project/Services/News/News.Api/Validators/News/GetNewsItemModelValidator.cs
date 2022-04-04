using FluentValidation;
using Foundation.Extensions.Validators;
using News.Api.ServicesModels.News;

namespace News.Api.Validators.News
{
    public class GetNewsItemModelValidator : BaseServiceModelValidator<GetNewsItemServiceModel>
    {
        public GetNewsItemModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
