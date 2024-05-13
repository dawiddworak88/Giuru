using System;

namespace Catalog.Api.v1.Categories.RequestModels
{
    public class SaveCategorySchemaRequestModel
    {
        public Guid? CategoryId { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
    }
}
