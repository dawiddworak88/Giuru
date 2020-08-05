using Foundation.Extensions.Models;
using System;

namespace Catalog.Api.v1.Areas.Schemas.Models
{
    public class GetSchemaModel : BaseServiceModel
    {
        public Guid? Id { get; set; }
    }
}
