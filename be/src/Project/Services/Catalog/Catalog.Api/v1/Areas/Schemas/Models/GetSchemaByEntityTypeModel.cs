using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.v1.Areas.Schemas.Models
{
    public class GetSchemaByEntityTypeModel : BaseServiceModel
    {
        public Guid Id { get; set; }
        public Guid? EntityTypeId { get; set; }
    }
}
