using Foundation.ApiExtensions.Models.Request;
using System;

namespace Catalog.Api.v1.Categories.RequestModels
{
    public class CategorySchemaRequestModel : RequestModelBase
    {        
        public string Schema { get; set; }
        public string UiSchema { get; set; }
        public string Language { get; set; }
    }
}
