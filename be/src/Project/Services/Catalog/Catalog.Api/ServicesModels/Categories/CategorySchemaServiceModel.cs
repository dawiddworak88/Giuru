using System;

namespace Catalog.Api.ServicesModels.Categories
{
    public class CategorySchemaServiceModel
    {
        public Guid Id { get; set; }
        public Guid CategoryId { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public DateTime LastModifiedDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Language { get; set; }
    }
}
