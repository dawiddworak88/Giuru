using Client.Api.ServicesModels.FieldOptions;
using FluentValidation;
using Foundation.Extensions.Validators;

namespace Client.Api.Validators.FieldOptions
{
    public class GetFieldOptionModelValidator : BaseServiceModelValidator<GetFieldOptionServiceModel>
    {
        public GetFieldOptionModelValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}
