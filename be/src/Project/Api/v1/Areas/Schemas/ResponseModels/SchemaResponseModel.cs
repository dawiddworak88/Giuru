using Foundation.ApiExtensions.Models.Response;
using Foundation.TenantDatabase.Areas.Schemas.Entities;
using System;

namespace Api.v1.Areas.Schemas.ResponseModels
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
            this.Schema = schema.JsonSchema;
            this.UiSchema = schema.UiSchema;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Schema { get; set; }
        public string UiSchema { get; set; }
    }
}
