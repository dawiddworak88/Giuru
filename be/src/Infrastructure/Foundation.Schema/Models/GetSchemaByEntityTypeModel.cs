using Foundation.Extensions.Models;
using System;

namespace Foundation.Schema.Models
{
    public class GetSchemaByEntityTypeModel : BaseServiceModel
    {
        public Guid? EntityTypeId { get; set; }
    }
}
