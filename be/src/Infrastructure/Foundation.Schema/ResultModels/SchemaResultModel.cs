using Foundation.Extensions.Models;

namespace Foundation.Schema.ResultModels
{
    public class SchemaResultModel : BaseServiceResultModel
    {
        public Database.Areas.Schemas.Entities.Schema Schema { get; set; }
    }
}
