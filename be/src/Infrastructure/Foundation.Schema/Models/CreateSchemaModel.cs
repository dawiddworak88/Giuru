using Foundation.Extensions.Models;
using Newtonsoft.Json.Linq;
using System;

namespace Foundation.Schema.Models
{
    public class CreateSchemaModel : BaseServiceModel
    {
        public string Name { get; set; }
        public Guid? EntityTypeId { get; set; }
        public JObject JsonSchema { get; set; }
        public JObject UiSchema { get; set; }
    }
}
