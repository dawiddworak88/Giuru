using Foundation.Extensions.Models;

namespace Foundation.Schema.ResultModels
{
    public class SchemaResultModel : BaseServiceResultModel
    {
        public TenantDatabase.Areas.Schemas.Entities.Schema Schema { get; set; }
    }
}
