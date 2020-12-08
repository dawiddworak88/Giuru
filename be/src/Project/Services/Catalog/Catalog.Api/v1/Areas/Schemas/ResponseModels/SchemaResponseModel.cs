using Foundation.ApiExtensions.Models.Response;
using Catalog.Api.Infrastructure.Schemas.Entities;

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
        }

        public string Name { get; set; }
        public string JsonSchema { get; set; }
        public string UiSchema { get; set; }
    }
}
