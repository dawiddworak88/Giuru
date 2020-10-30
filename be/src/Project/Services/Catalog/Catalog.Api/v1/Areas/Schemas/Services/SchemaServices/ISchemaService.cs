using Catalog.Api.v1.Areas.Schemas.Models;
using Catalog.Api.v1.Areas.Schemas.ResultModels;
using System.Threading.Tasks;

namespace Catalog.Api.v1.Areas.Schemas.Services.SchemaServices
{
    public interface ISchemaService
    {
        Task<SchemaResultModel> CreateAsync(CreateSchemaModel model);
        Task<SchemaResultModel> GetByIdAsync(GetSchemaModel getSchemaModel);
        Task<SchemaResultModel> GetByEntityTypeIdAsync(GetSchemaByEntityTypeModel getSchemaModel);
    }
}
