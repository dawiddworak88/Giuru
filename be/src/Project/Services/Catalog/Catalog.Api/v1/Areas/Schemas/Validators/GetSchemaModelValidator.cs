using Foundation.Extensions.Validators;
using Catalog.Api.v1.Areas.Schemas.Models;
using FluentValidation;

namespace Catalog.Api.v1.Areas.Schemas.Validators
{
    public class GetSchemaModelValidator : BaseAuthorizedServiceModelValidator<GetSchemaModel>
    {
        public GetSchemaModelValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}
