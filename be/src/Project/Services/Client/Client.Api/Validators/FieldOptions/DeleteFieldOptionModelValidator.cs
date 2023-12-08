using Client.Api.ServicesModels.FieldOptions;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.FieldOptions
{
    public class DeleteFieldOptionModelValidator : BaseServiceModelValidator<DeleteClientFieldOptionServiceModel>
    {
        public DeleteFieldOptionModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
