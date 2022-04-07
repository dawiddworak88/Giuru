using FluentValidation;
using Foundation.Extensions.Validators;
using News.Api.ServicesModels.News;

namespace News.Api.Validators.News
{
    public class DeleteNewsItemModelValidator : BaseServiceModelValidator<DeleteNewsItemServiceModel>
    {
        public DeleteNewsItemModelValidator()
        {
            this.RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
