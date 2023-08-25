using System;

namespace Catalog.Api.ServicesModels.Categories
{
    public class SchemaServiceModel
    {
        public Guid? Id { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public string Language { get; set; }
    }
}
