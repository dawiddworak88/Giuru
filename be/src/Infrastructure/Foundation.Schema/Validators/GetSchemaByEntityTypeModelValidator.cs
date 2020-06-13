using Foundation.Extensions.Validators;
using Foundation.Schema.Models;
using FluentValidation;

namespace Foundation.Schema.Validators
{
    public class GetSchemaByEntityTypeModelValidator : BaseServiceModelValidator<GetSchemaByEntityTypeModel>
    {
        public GetSchemaByEntityTypeModelValidator()
        {
            RuleFor(x => x.EntityTypeId).NotNull();
        }
    }
}
