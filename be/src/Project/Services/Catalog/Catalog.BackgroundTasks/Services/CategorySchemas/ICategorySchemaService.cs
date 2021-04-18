using System.Threading.Tasks;

namespace Catalog.BackgroundTasks.Services.CategorySchemas
{
    public interface ICategorySchemaService
    {
        Task RebuildCategorySchemasAsync();
    }
}
