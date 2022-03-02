using FluentValidation;
using Foundation.Extensions.Validators;
using News.Api.ServicesModels.News;

namespace News.Api.Validators.News
{
    public class CreateNewsItemModelValidator : BaseServiceModelValidator<CreateNewsItemServiceModel>
    {
        public CreateNewsItemModelValidator()
        {
            this.RuleFor(x => x.Title).NotNull().NotEmpty();
            this.RuleFor(x => x.Description).NotNull().NotEmpty();
            this.RuleFor(x => x.Content).NotNull().NotEmpty();
            this.RuleFor(x => x.CategoryId).NotNull().NotEmpty();
            this.RuleFor(x => x.ThumbImageId).NotNull().NotEmpty();
            this.RuleFor(x => x.HeroImageId).NotNull().NotEmpty();
        }
    }
}
