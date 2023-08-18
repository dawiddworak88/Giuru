using Catalog.Api.ServicesModels.Categories;
using Foundation.GenericRepository.Paginations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Catalog.Api.Services.Categories
{
    public interface ICategoriesService
    {
        PagedResults<IEnumerable<CategoryServiceModel>> Get(GetCategoriesServiceModel model);
        CategoryServiceModel Get(GetCategoryServiceModel model);
        Task DeleteAsync(DeleteCategoryServiceModel model);
        Task<CategoryServiceModel> UpdateAsync(UpdateCategoryServiceModel model);
        Task<CategoryServiceModel> CreateAsync(CreateCategoryServiceModel model);
        Task<CategorySchemaServiceModel> UpdateCategorySchemaAsync(UpdateCategorySchemaServiceModel model);
        Task<CategorySchemaServiceModel> GetCategorySchemaAsync(GetCategorySchemaServiceModel model);
        IEnumerable<CategorySchemaServiceModel> GetCategorySchemas(GetCategorySchemasServiceModel model);
    }
}
