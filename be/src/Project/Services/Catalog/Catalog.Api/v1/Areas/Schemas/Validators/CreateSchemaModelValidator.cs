using FluentValidation;
using Foundation.Extensions.Validators;
using Catalog.Api.v1.Areas.Schemas.Models;

namespace Catalog.Api.v1.Areas.Schemas.Validators
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
