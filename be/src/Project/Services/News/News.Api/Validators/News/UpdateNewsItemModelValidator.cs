using FluentValidation;
using Foundation.Extensions.Validators;
using News.Api.ServicesModels.News;

namespace News.Api.Validators.News
{
    public class UpdateNewsItemModelValidator : BaseServiceModelValidator<UpdateNewsItemServiceModel>
    {
        public UpdateNewsItemModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
            this.RuleFor(x => x.Title).NotNull().NotEmpty();
            this.RuleFor(x => x.Description).NotNull().NotEmpty();
            this.RuleFor(x => x.Content).NotNull().NotEmpty();
            this.RuleFor(x => x.CategoryId).NotNull().NotEmpty();
            this.RuleFor(x => x.ThumbnailImageId).NotNull().NotEmpty();
        }
    }
}
