using Foundation.Extensions.Models;
using System;

namespace Foundation.Schema.Models
{
    public class CreateSchemaModel : BaseServiceModel
    {
        public string Name { get; set; }
        public Guid? EntityTypeId { get; set; }
        public string JsonSchema { get; set; }
        public string UiSchema { get; set; }
    }
}
