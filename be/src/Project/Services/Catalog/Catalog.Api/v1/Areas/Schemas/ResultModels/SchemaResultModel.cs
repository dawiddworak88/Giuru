using Catalog.Api.Infrastructure.Schemas.Entities;
using Foundation.Extensions.Models;

namespace Catalog.Api.v1.Areas.Schemas.ResultModels
{
    public class SchemaResultModel : BaseServiceResultModel
    {
        public Schema Schema { get; set; }
    }
}
