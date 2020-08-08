using Foundation.Extensions.Validators;
using Catalog.Api.v1.Areas.Schemas.Models;
using FluentValidation;

namespace Catalog.Api.v1.Areas.Schemas.Validators
{
    public class GetSchemaByEntityTypeModelValidator : BaseServiceModelValidator<GetSchemaByEntityTypeModel>
    {
        public GetSchemaByEntityTypeModelValidator()
        {
            RuleFor(x => x.EntityTypeId).NotNull();
        }
    }
}
