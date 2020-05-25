using Foundation.ApiExtensions.Models.Request;
using System;

namespace Api.v1.Areas.Schemas.RequestModels
{
    public class SchemaRequestModel : BaseRequestModel
    {
        public string Name { get; set; }
        public Guid? EntityTypeId { get; set; }
        public string JsonSchema { get; set; }
        public string UiSchema { get; set; }
    }
}
