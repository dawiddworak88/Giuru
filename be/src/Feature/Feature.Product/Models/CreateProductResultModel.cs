using FluentValidation.Results;

namespace Feature.Product.Models
{
    public class CreateProductResultModel
    {
        public Foundation.TenantDatabase.Areas.Products.Entities.Product Product { get; set; }
        public ValidationResult ValidationResult { get; set; }
    }
}
