using Foundation.ApiExtensions.Models.Response;
using Foundation.Database.Areas.Schemas.Entities;
using System;

namespace Catalog.Api.v1.Areas.Schemas.ResponseModels
{
    public class SchemaResponseModel : BaseResponseModel
    {
        public SchemaResponseModel()
        { 
        }

        public SchemaResponseModel(Schema schema)
        {
            this.Id = schema.Id;
            this.Name = schema.Name;
            this.JsonSchema = schema.JsonSchema;
            this.UiSchema = schema.UiSchema;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string JsonSchema { get; set; }
        public string UiSchema { get; set; }
    }
}
