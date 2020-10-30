using Foundation.ApiExtensions.Models.Request;
using System;

namespace Catalog.Api.v1.Areas.Schemas.RequestModels
{
    public class SchemaRequestModel : RequestModelBase
    {
        public string Name { get; set; }
        public Guid? EntityTypeId { get; set; }
        public string JsonSchema { get; set; }
        public string UiSchema { get; set; }
    }
}
