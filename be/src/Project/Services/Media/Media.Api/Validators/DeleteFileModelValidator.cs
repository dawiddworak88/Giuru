using FluentValidation;
using Foundation.Extensions.Validators;
using Media.Api.ServicesModels;

namespace Media.Api.Validators
{
    public class DeleteFileModelValidator : BaseServiceModelValidator<DeleteFileServiceModel>
    {
        public DeleteFileModelValidator()
        {
            this.RuleFor(x => x.MediaId).NotNull().NotEmpty();
        }
    }
}
