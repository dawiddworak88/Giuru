using Foundation.Extensions.Validators;
using Foundation.Schema.Models;
using FluentValidation;

namespace Foundation.Schema.Validators
{
    public class GetSchemaModelValidator : BaseServiceModelValidator<GetSchemaModel>
    {
        public GetSchemaModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
