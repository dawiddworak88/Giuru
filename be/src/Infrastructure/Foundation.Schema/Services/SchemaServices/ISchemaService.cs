using Foundation.Schema.Models;
using Foundation.Schema.ResultModels;
using System.Threading.Tasks;

namespace Foundation.Schema.Services.SchemaServices
{
    public interface ISchemaService
    {
        Task<SchemaResultModel> CreateAsync(CreateSchemaModel model);
        Task<SchemaResultModel> GetByIdAsync(GetSchemaModel getSchemaModel);
    }
}
