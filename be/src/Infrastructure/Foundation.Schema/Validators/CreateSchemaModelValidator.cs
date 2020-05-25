using FluentValidation;
using Foundation.Extensions.Validators;
using Foundation.Schema.Models;

namespace Foundation.Schema.Validators
{
    public class CreateSchemaModelValidator : BaseServiceModelValidator<CreateSchemaModel>
    {
        public CreateSchemaModelValidator()
        {
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.JsonSchema).NotNull().NotEmpty();
            RuleFor(x => x.EntityTypeId).NotNull();
        }
    }
}
