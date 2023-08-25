using System;
using System.Data.SqlTypes;
using System.Reflection.Metadata.Ecma335;

namespace Catalog.Api.v1.Categories.RequestModels
{
    public class SaveCategorySchemaRequestModel
    {
        public Guid? CategoryId { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
    }
}
